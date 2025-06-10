using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(100)]
        public string Username { get; set; }

        [Required, MaxLength(100)]
        public string Email { get; set; }

        [Required]
        public string PasswordHash { get; set; }

        // Роль: "User", "Librarian", "Admin"
        [Required, MaxLength(20)]
        public string Role { get; set; }

        public ICollection<Reservation> Reservations { get; set; }
        public ICollection<BookReview> Reviews { get; set; }
        public ICollection<AuditLog> AuditLogs { get; set; }
    }
}