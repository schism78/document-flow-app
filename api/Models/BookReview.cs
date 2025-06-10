using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class BookReview
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Text { get; set; }

        [Range(1, 5)]
        public int Rating { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [ForeignKey("User")]
        [Required]
        public int UserId { get; set; }
        public User User { get; set; }

        [ForeignKey("Book")]
        [Required]
        public int BookId { get; set; }
        public Book Book { get; set; }
    }