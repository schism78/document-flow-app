using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Departments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Departments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FullName = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    Login = table.Column<string>(type: "text", nullable: false),
                    PasswordHash = table.Column<string>(type: "text", nullable: false),
                    Role = table.Column<string>(type: "text", nullable: false),
                    DepartmentId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_Departments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Departments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Documents",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Title = table.Column<string>(type: "text", nullable: false),
                    Status = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    SenderUserId = table.Column<int>(type: "integer", nullable: false),
                    CurrentUserId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Documents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Documents_Users_CurrentUserId",
                        column: x => x.CurrentUserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Documents_Users_SenderUserId",
                        column: x => x.SenderUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DocumentFiles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FilePath = table.Column<string>(type: "text", nullable: false),
                    DocumentId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentFiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DocumentFiles_Documents_DocumentId",
                        column: x => x.DocumentId,
                        principalTable: "Documents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DocumentRoutes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    DocumentId = table.Column<int>(type: "integer", nullable: false),
                    FromUserId = table.Column<int>(type: "integer", nullable: false),
                    ToUserId = table.Column<int>(type: "integer", nullable: false),
                    Comment = table.Column<string>(type: "text", nullable: true),
                    SentAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Action = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentRoutes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DocumentRoutes_Documents_DocumentId",
                        column: x => x.DocumentId,
                        principalTable: "Documents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DocumentRoutes_Users_FromUserId",
                        column: x => x.FromUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DocumentRoutes_Users_ToUserId",
                        column: x => x.ToUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Администрация" },
                    { 2, "Методический отдел" },
                    { 3, "Отдел комплектования" },
                    { 4, "Канцелярия" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "DepartmentId", "Email", "FullName", "Login", "PasswordHash", "Role" },
                values: new object[,]
                {
                    { 1, 4, "secretary@library.local", "Иванова Светлана Петровна", "secretary", "hashedpassword1", "Секретарь" },
                    { 2, 1, "director@library.local", "Сидоров Алексей Михайлович", "director", "hashedpassword2", "Директор" },
                    { 3, 2, "methodist@library.local", "Петрова Мария Алексеевна", "methodist", "hashedpassword3", "Исполнитель" },
                    { 4, 3, "supply@library.local", "Козлов Дмитрий Сергеевич", "supply", "hashedpassword4", "Исполнитель" },
                    { 5, 2, "method2@library.local", "Андреева Елена Викторовна", "method2", "hashedpassword5", "Исполнитель" }
                });

            migrationBuilder.InsertData(
                table: "Documents",
                columns: new[] { "Id", "CreatedAt", "CurrentUserId", "SenderUserId", "Status", "Title" },
                values: new object[,]
                {
                    { 1, new DateTime(2024, 5, 1, 9, 0, 0, 0, DateTimeKind.Utc), 3, 1, "InProgress", "Запрос отчета по мероприятиям" },
                    { 2, new DateTime(2024, 5, 3, 10, 0, 0, 0, DateTimeKind.Utc), 4, 1, "SentToExecutor", "Закупка оборудования" },
                    { 3, new DateTime(2024, 5, 5, 12, 0, 0, 0, DateTimeKind.Utc), 2, 1, "Approved", "Распоряжение о внутреннем собрании" }
                });

            migrationBuilder.InsertData(
                table: "DocumentFiles",
                columns: new[] { "Id", "DocumentId", "FilePath" },
                values: new object[,]
                {
                    { 1, 1, "https://storage.yandexcloud.net/library-docs/doc1.pdf" },
                    { 2, 1, "https://storage.yandexcloud.net/library-docs/doc1-appendix.pdf" },
                    { 3, 2, "https://storage.yandexcloud.net/library-docs/doc2.pdf" },
                    { 4, 3, "https://storage.yandexcloud.net/library-docs/doc3.pdf" }
                });

            migrationBuilder.InsertData(
                table: "DocumentRoutes",
                columns: new[] { "Id", "Action", "Comment", "DocumentId", "FromUserId", "SentAt", "ToUserId" },
                values: new object[,]
                {
                    { 1, 0, "Входящее письмо из департамента культуры", 1, 1, new DateTime(2024, 5, 1, 9, 30, 0, 0, DateTimeKind.Utc), 2 },
                    { 2, 0, "Подготовьте, пожалуйста, отчет к утру", 1, 2, new DateTime(2024, 5, 1, 10, 0, 0, 0, DateTimeKind.Utc), 3 },
                    { 3, 0, "Нужна смета на оборудование", 2, 1, new DateTime(2024, 5, 3, 10, 30, 0, 0, DateTimeKind.Utc), 2 },
                    { 4, 2, "Одобрено директором", 3, 1, new DateTime(2024, 5, 5, 12, 30, 0, 0, DateTimeKind.Utc), 2 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_DocumentFiles_DocumentId",
                table: "DocumentFiles",
                column: "DocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentRoutes_DocumentId",
                table: "DocumentRoutes",
                column: "DocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentRoutes_FromUserId",
                table: "DocumentRoutes",
                column: "FromUserId");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentRoutes_ToUserId",
                table: "DocumentRoutes",
                column: "ToUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Documents_CurrentUserId",
                table: "Documents",
                column: "CurrentUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Documents_SenderUserId",
                table: "Documents",
                column: "SenderUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_DepartmentId",
                table: "Users",
                column: "DepartmentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DocumentFiles");

            migrationBuilder.DropTable(
                name: "DocumentRoutes");

            migrationBuilder.DropTable(
                name: "Documents");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Departments");
        }
    }
}
