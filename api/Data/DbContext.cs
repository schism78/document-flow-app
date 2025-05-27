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

            // Вызов метода сидирования
            SeedData(modelBuilder);
        }

        private void SeedData(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Department>().HasData(
                new Department { Id = 1, Name = "Отдел кадров" },
                new Department { Id = 2, Name = "Юридический отдел" },
                new Department { Id = 3, Name = "Финансовый отдел" }
            );
            
            string ?bucket = Environment.GetEnvironmentVariable("BUCKET_NAME");

            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = 1,
                    FullName = "Иван Иванов",
                    Email = "ivanov@company.ru",
                    Login = "aboba1",
                    PasswordHash = "123",
                    Role = "Секретарь",
                    DepartmentId = 1
                },
                new User
                {
                    Id = 2,
                    FullName = "Сергей Петров",
                    Email = "petrov@company.ru",
                    Login = "aboba2",
                    PasswordHash = "1234",
                    Role = "Директор",
                    DepartmentId = 2
                },
                new User
                {
                    Id = 3,
                    FullName = "Мария Смирнова",
                    Email = "smirnova@company.ru",
                    Login = "aboba3",
                    PasswordHash = "12345",
                    Role = "Исполнитель",
                    DepartmentId = 3
                }
            );

            modelBuilder.Entity<Document>().HasData(
                
                new Document
                {
                    Id = 1,
                    Title = "Заявление на отпуск",
                    FileUrl = $"https://{bucket}.storage.yandexcloud.net/documents/text.txt",
                    CreatedAt = DateTime.SpecifyKind(new DateTime(2024, 1, 15), DateTimeKind.Utc),
                    Status = DocumentStatus.SentToDirector,
                    SenderUserId = 1,
                    CurrentUserId = 2
                }
            );

            modelBuilder.Entity<DocumentRoute>().HasData(
                new DocumentRoute
                {
                    Id = 1,
                    DocumentId = 1,
                    FromUserId = 1,
                    ToUserId = 2,
                    Comment = "Прошу согласовать отпуск",
                    SentAt = DateTime.SpecifyKind(new DateTime(2024, 1, 16), DateTimeKind.Utc),
                    Action = DocumentRouteAction.Forward
                }
            );
        }

    }
}