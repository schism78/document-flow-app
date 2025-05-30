using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api.Models
{
    public class DocumentFile
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string FilePath { get; set; } = null!;

        public int DocumentId { get; set; }

        [ForeignKey("DocumentId")]
        public Document Document { get; set; }
    }
}
