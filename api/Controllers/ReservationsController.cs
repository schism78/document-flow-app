using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using api.Data;
using api.Models;
using api.Dtos;

namespace api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReservationsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ReservationsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/reservations
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Reservation>>> GetReservations()
        {
            return await _context.Reservations
                .Include(r => r.Book)
                .Include(r => r.User)
                .ToListAsync();
        }

        // GET: api/reservations/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Reservation>> GetReservation(int id)
        {
            var reservation = await _context.Reservations
                .Include(r => r.Book)
                .Include(r => r.User)
                .FirstOrDefaultAsync(r => r.Id == id);

            return reservation == null ? NotFound() : reservation;
        }

        // GET api/reservations/filter?userId=1&bookId=2
        [HttpGet("filter")]
        public async Task<ActionResult<IEnumerable<Reservation>>> GetReservations([FromQuery] int? userId, [FromQuery] int? bookId)
        {
            IQueryable<Reservation> query = _context.Reservations.Include(r => r.Book).Include(r => r.User);
            if (userId.HasValue)
                query = query.Where(r => r.UserId == userId.Value);
            if (bookId.HasValue)
                query = query.Where(r => r.BookId == bookId.Value);
            var reservations = await query.ToListAsync();
            return reservations;
        }

        // POST: api/reservations
        [HttpPost]
        public async Task<ActionResult<Reservation>> CreateReservation(CreateReservationDto reservationDto)
        {
            var book = await _context.Books.FindAsync(reservationDto.BookId);
            if (book == null)
                return NotFound("Книга не найдена");
            if (book.AvailableCopies <= 0)
                return BadRequest("Нет доступных экземпляров книги");
            // Проверка ReturnBy > текущей даты
            if (reservationDto.ReturnBy <= DateTime.UtcNow)
                return BadRequest("Дата возврата должна быть позже текущей даты");
            var reservation = new Reservation
            {
                BookId = reservationDto.BookId,
                UserId = reservationDto.UserId, // Замените на актуальный ID пользователя
                ReturnBy = reservationDto.ReturnBy,
                Status = "Reserved",
                ReservedAt = DateTime.UtcNow
            };
            book.AvailableCopies--;
            _context.Reservations.Add(reservation);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetReservation), new { id = reservation.Id }, reservation);
        }

        // PUT: api/reservations/5/return
        [HttpPut("{id}/return")]
        public async Task<IActionResult> ReturnBook(int id)
        {
            var reservation = await _context.Reservations
                .Include(r => r.Book)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (reservation == null)
                return NotFound("Бронирование не найдено");

            if (reservation.Status == "Returned")
                return BadRequest("Книга уже возвращена");

            reservation.Status = "Returned";
            reservation.ReturnedAt = DateTime.UtcNow;

            reservation.Book.AvailableCopies++;

            _context.Entry(reservation).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok("Книга успешно возвращена");
        }

        // DELETE: api/reservations/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReservation(int id)
        {
            var reservation = await _context.Reservations
                .Include(r => r.Book)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (reservation == null)
                return NotFound();

            if (reservation.Status == "Reserved")
            {
                reservation.Book.AvailableCopies++;
            }

            _context.Reservations.Remove(reservation);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
