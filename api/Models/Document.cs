using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api.Models
{
    public class Document
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; } = null!;

        [Required]
        public string FileUrl { get; set; } = null!;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Required]
        public DocumentStatus Status { get; set; }

        [Required]
        public int SenderUserId { get; set; } // Кто создал (секретарь)

        [ForeignKey("SenderUserId")]
        public User SenderUser { get; set; }

        public int? CurrentUserId { get; set; } // Кто сейчас держит документ

        [ForeignKey("CurrentUserId")]
        public User? CurrentUser { get; set; }
    }
}
