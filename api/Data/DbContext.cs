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
        public DbSet<AuditLog> AuditLogs { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<BookReview> BookReviews { get; set; }
        public DbSet<BookStatistic> BookStatistics { get; set; }
        public DbSet<FileAttachment> FileAttachments { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<User> Users { get; set; }
    }
}