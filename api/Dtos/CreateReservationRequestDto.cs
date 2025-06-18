using System;
namespace api.Dtos
{
    public class CreateReservationDto
    {
        public int BookId { get; set; }
        public int UserId { get; set; }
        public DateTime ReturnBy { get; set; }
    }
}