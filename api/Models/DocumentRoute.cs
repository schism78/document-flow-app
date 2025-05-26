using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using api.Models;

namespace api.Models
{
    public class DocumentRoute
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int DocumentId { get; set; }

        [ForeignKey("DocumentId")]
        public Document Document { get; set; }

        [Required]
        public int FromUserId { get; set; }

        [ForeignKey("FromUserId")]
        public User FromUser { get; set; }

        [Required]
        public int ToUserId { get; set; }

        [ForeignKey("ToUserId")]
        public User ToUser { get; set; }

        public string? Comment { get; set; }

        public DateTime SentAt { get; set; } = DateTime.UtcNow;

        [Required]
        public DocumentRouteAction Action { get; set; }
    }
}
