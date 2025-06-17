using System;
namespace api.Models
{
    public class CreateReservationDto
    {
        public int BookId { get; set; }
        public int UserId { get; set; }
        public DateTime ReturnBy { get; set; }
    }
}