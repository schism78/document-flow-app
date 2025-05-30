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

        public string Status { get; set; } = "InProgress";

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public int SenderUserId { get; set; }

        [ForeignKey("SenderUserId")]
        public User SenderUser { get; set; }

        public int? CurrentUserId { get; set; }

        [ForeignKey("CurrentUserId")]
        public User? CurrentUser { get; set; }

        public ICollection<DocumentFile> Files { get; set; } = new List<DocumentFile>();
    }
}