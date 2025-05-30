using api.Data;
using Microsoft.EntityFrameworkCore;
using DotNetEnv;
using Amazon.S3;
using Amazon.S3.Model;
using YandexCloudStorageApp.Services;

var builder = WebApplication.CreateBuilder(args);

Env.Load();

string? dbName = Environment.GetEnvironmentVariable("DB_NAME");
string? dbUser = Environment.GetEnvironmentVariable("DB_USERNAME");
string? dbPass = Environment.GetEnvironmentVariable("DB_PASSWORD");

string connectionString = $"Host=localhost;Port=5432;Database={dbName};Username={dbUser};Password={dbPass}";

// Регистрируем DbContext с PostgreSQL
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddSingleton<YandexStorageService>();
builder.Services.AddControllers();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});
builder.Services.AddEndpointsApiExplorer();

string? serviceUrl = Environment.GetEnvironmentVariable("SERVICE_URL");
string? accessKey = Environment.GetEnvironmentVariable("ACCESS_KEY");
string? secretKey = Environment.GetEnvironmentVariable("SECRET_KEY");

builder.Services.AddSingleton<IAmazonS3>(sp =>
{
    var s3Config = new AmazonS3Config
    {
        ServiceURL = serviceUrl, // Указываем URL Yandex Object Storage
        ForcePathStyle = true    // Требуется для совместимости с Yandex Cloud
    };
    return new AmazonS3Client(accessKey, secretKey, s3Config);
});

var app = builder.Build();

app.MapControllers();
app.UseCors();
app.Run();
