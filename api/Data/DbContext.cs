using Microsoft.EntityFrameworkCore;
using api.Models;
using DotNetEnv;
using Yandex.Cloud.Storage.V1;

namespace api.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Document> Documents { get; set; }
        public DbSet<DocumentRoute> DocumentRoutes { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Seed Departments
            modelBuilder.Entity<Department>().HasData(
                new Department { Id = 1, Name = "Администрация" },
                new Department { Id = 2, Name = "Методический отдел" },
                new Department { Id = 3, Name = "Отдел комплектования" },
                new Department { Id = 4, Name = "Канцелярия" }
            );

            // Seed Users
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = 1,
                    FullName = "Иванова Светлана Петровна",
                    Email = "secretary@library.local",
                    Login = "secretary",
                    PasswordHash = "hashedpassword1", // заглушка
                    Role = "Секретарь",
                    DepartmentId = 4
                },
                new User
                {
                    Id = 2,
                    FullName = "Сидоров Алексей Михайлович",
                    Email = "director@library.local",
                    Login = "director",
                    PasswordHash = "hashedpassword2",
                    Role = "Директор",
                    DepartmentId = 1
                },
                new User
                {
                    Id = 3,
                    FullName = "Петрова Мария Алексеевна",
                    Email = "methodist@library.local",
                    Login = "methodist",
                    PasswordHash = "hashedpassword3",
                    Role = "Исполнитель",
                    DepartmentId = 2
                },
                new User
                {
                    Id = 4,
                    FullName = "Козлов Дмитрий Сергеевич",
                    Email = "supply@library.local",
                    Login = "supply",
                    PasswordHash = "hashedpassword4",
                    Role = "Исполнитель",
                    DepartmentId = 3
                }
            );

            // Seed Documents
            modelBuilder.Entity<Document>().HasData(
                new Document
                {
                    Id = 1,
                    Title = "Запрос отчета по мероприятиям",
                    FileUrl = "https://storage.yandexcloud.net/library-docs/doc1.pdf",
                    CreatedAt = new DateTime(2024, 5, 1, 9, 0, 0, DateTimeKind.Utc),
                    Status = DocumentStatus.InProgress,
                    SenderUserId = 1, // секретарь
                    CurrentUserId = 3 // у исполнителя (методист)
                }
            );

            // Seed DocumentRoutes
            modelBuilder.Entity<DocumentRoute>().HasData(
                new DocumentRoute
                {
                    Id = 1,
                    DocumentId = 1,
                    FromUserId = 1, // Секретарь
                    ToUserId = 2,   // Директор
                    SentAt = new DateTime(2024, 5, 1, 9, 30, 0, DateTimeKind.Utc),
                    Action = DocumentRouteAction.Forward,
                    Comment = "Входящее письмо из департамента культуры"
                },
                new DocumentRoute
                {
                    Id = 2,
                    DocumentId = 1,
                    FromUserId = 2, // Директор
                    ToUserId = 3,   // Методист
                    SentAt = new DateTime(2024, 5, 1, 9, 30, 0, DateTimeKind.Utc),
                    Action = DocumentRouteAction.Forward,
                    Comment = "Подготовьте, пожалуйста, отчет к утру"
                }
            );
        }

    }
}