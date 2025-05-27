using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class AddSeedDataFixed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Отдел кадров" },
                    { 2, "Юридический отдел" },
                    { 3, "Финансовый отдел" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "DepartmentId", "Email", "FullName", "Role" },
                values: new object[,]
                {
                    { 1, 1, "ivanov@company.ru", "Иван Иванов", "Секретарь" },
                    { 2, 2, "petrov@company.ru", "Сергей Петров", "Директор" },
                    { 3, 3, "smirnova@company.ru", "Мария Смирнова", "Исполнитель" }
                });

            migrationBuilder.InsertData(
                table: "Documents",
                columns: new[] { "Id", "CreatedAt", "CurrentUserId", "FileUrl", "SenderUserId", "Status", "Title" },
                values: new object[] { 1, new DateTime(2024, 1, 15, 0, 0, 0, 0, DateTimeKind.Utc), 2, "https://flow1.storage.yandexcloud.net/documents/text.txt", 1, 1, "Заявление на отпуск" });

            migrationBuilder.InsertData(
                table: "DocumentRoutes",
                columns: new[] { "Id", "Action", "Comment", "DocumentId", "FromUserId", "SentAt", "ToUserId" },
                values: new object[] { 1, 0, "Прошу согласовать отпуск", 1, 1, new DateTime(2024, 1, 16, 0, 0, 0, 0, DateTimeKind.Utc), 2 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "DocumentRoutes",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Documents",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 2);
        }
    }
}
