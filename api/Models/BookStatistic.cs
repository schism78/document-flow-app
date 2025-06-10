using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


public class BookStatistic
    {
        [Key, ForeignKey("Book")]
        public int BookId { get; set; }
        public Book Book { get; set; }

        public int TotalReservations { get; set; }

        public int TotalReviews { get; set; }

        public double AverageRating { get; set; }
    }