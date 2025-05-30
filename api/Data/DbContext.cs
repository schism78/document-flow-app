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
        public DbSet<DocumentFile> DocumentFiles { get; set; }
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
            PasswordHash = "hashedpassword1",
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
        },
        new User
        {
            Id = 5,
            FullName = "Андреева Елена Викторовна",
            Email = "method2@library.local",
            Login = "method2",
            PasswordHash = "hashedpassword5",
            Role = "Исполнитель",
            DepartmentId = 2
        }
    );

    // Seed Documents
    modelBuilder.Entity<Document>().HasData(
        new Document
        {
            Id = 1,
            Title = "Запрос отчета по мероприятиям",
            CreatedAt = new DateTime(2024, 5, 1, 9, 0, 0, DateTimeKind.Utc),
            Status = "InProgress",
            SenderUserId = 1,
            CurrentUserId = 3
        },
        new Document
        {
            Id = 2,
            Title = "Закупка оборудования",
            CreatedAt = new DateTime(2024, 5, 3, 10, 0, 0, DateTimeKind.Utc),
            Status = "SentToExecutor",
            SenderUserId = 1,
            CurrentUserId = 4
        },
        new Document
        {
            Id = 3,
            Title = "Распоряжение о внутреннем собрании",
            CreatedAt = new DateTime(2024, 5, 5, 12, 0, 0, DateTimeKind.Utc),
            Status = "Approved",
            SenderUserId = 1,
            CurrentUserId = 2
        }
    );

    // Seed DocumentFiles
    modelBuilder.Entity<DocumentFile>().HasData(
        new DocumentFile { Id = 1, DocumentId = 1, FilePath = "https://storage.yandexcloud.net/library-docs/doc1.pdf" },
        new DocumentFile { Id = 2, DocumentId = 1, FilePath = "https://storage.yandexcloud.net/library-docs/doc1-appendix.pdf" },
        new DocumentFile { Id = 3, DocumentId = 2, FilePath = "https://storage.yandexcloud.net/library-docs/doc2.pdf" },
        new DocumentFile { Id = 4, DocumentId = 3, FilePath = "https://storage.yandexcloud.net/library-docs/doc3.pdf" }
    );

    // Seed DocumentRoutes
    modelBuilder.Entity<DocumentRoute>().HasData(
    new DocumentRoute
    {
        Id = 1,
        DocumentId = 1,
        FromUserId = 1,
        ToUserId = 2,
        SentAt = new DateTime(2024, 5, 1, 9, 30, 0, DateTimeKind.Utc),
        Action = DocumentRouteAction.Forward,
        Comment = "Входящее письмо из департамента культуры"
    },
    new DocumentRoute
    {
        Id = 2,
        DocumentId = 1,
        FromUserId = 2,
        ToUserId = 3,
        SentAt = new DateTime(2024, 5, 1, 10, 0, 0, DateTimeKind.Utc),
        Action = DocumentRouteAction.Forward,
        Comment = "Подготовьте, пожалуйста, отчет к утру"
    },
    new DocumentRoute
    {
        Id = 3,
        DocumentId = 2,
        FromUserId = 1,
        ToUserId = 2,
        SentAt = new DateTime(2024, 5, 3, 10, 30, 0, DateTimeKind.Utc),
        Action = DocumentRouteAction.Forward,
        Comment = "Нужна смета на оборудование"
    },
    new DocumentRoute
    {
        Id = 4,
        DocumentId = 3,
        FromUserId = 1,
        ToUserId = 2,
        SentAt = new DateTime(2024, 5, 5, 12, 30, 0, DateTimeKind.Utc),
        Action = DocumentRouteAction.Approve,
        Comment = "Одобрено директором"
    }
    );
    }

    }
}