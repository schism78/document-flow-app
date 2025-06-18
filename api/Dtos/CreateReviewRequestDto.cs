using System.ComponentModel.DataAnnotations;
namespace api.Dtos
{
    public class BookReviewDto
    {
        [Required]
        public string Text { get; set; }
        [Range(1, 5)]
        public int Rating { get; set; }
        [Required]
        public int UserId { get; set; }
        [Required]
        public int BookId { get; set; }
    }
}