using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api.Models
{
    public class Book
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(300)]
        public string Title { get; set; }

        [MaxLength(200)]
        public string Author { get; set; }

        public string Annotation { get; set; } // Краткое описание

        [ForeignKey("Genre")]
        public int? GenreId { get; set; }
        public Genre Genre { get; set; }
        public int TotalCopies { get; set; }      // Общее количество экземпляров книги
        public int AvailableCopies { get; set; }    // Количество доступных копий

        public ICollection<Reservation> Reservations { get; set; }
        public ICollection<BookReview> Reviews { get; set; }
        public ICollection<FileAttachment> FileAttachments { get; set; }
        public BookStatistic Statistic { get; set; }
    }
}