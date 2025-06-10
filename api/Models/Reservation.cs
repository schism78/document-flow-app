using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api.Models
{
    public class Reservation
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Book")]
        [Required]
        public int BookId { get; set; }
        public Book Book { get; set; }

        [ForeignKey("User")]
        [Required]
        public int UserId { get; set; }
        public User User { get; set; }

        [Required]
        public DateTime ReservedAt { get; set; } = DateTime.UtcNow;

        [Required]
        public DateTime ReturnBy { get; set; }

        public DateTime? ReturnedAt { get; set; }

        [Required, MaxLength(20)]
        public string Status { get; set; } // "Reserved", "Returned", "Overdue"
    }
}
