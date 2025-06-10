using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using api.Data;
using api.Models;

namespace api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookStatisticsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public BookStatisticsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/bookstatistics/5
        [HttpGet("{bookId}")]
        public async Task<ActionResult<BookStatistic>> GetBookStatistic(int bookId)
        {
            var statistic = await _context.BookStatistics
                .Include(bs => bs.Book)
                .FirstOrDefaultAsync(bs => bs.BookId == bookId);

            if (statistic == null)
                return NotFound();

            return Ok(statistic);
        }

        // PUT: api/bookstatistics/5
        [HttpPut("{bookId}")]
        public async Task<IActionResult> UpdateBookStatistic(int bookId, BookStatistic updatedStatistic)
        {
            if (bookId != updatedStatistic.BookId)
                return BadRequest();

            var statistic = await _context.BookStatistics.FindAsync(bookId);
            if (statistic == null)
                return NotFound();

            statistic.TotalReservations = updatedStatistic.TotalReservations;
            statistic.TotalReviews = updatedStatistic.TotalReviews;
            statistic.AverageRating = updatedStatistic.AverageRating;

            _context.Entry(statistic).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // POST: api/bookstatistics
        [HttpPost]
        public async Task<ActionResult<BookStatistic>> CreateBookStatistic(BookStatistic statistic)
        {
            if (await _context.BookStatistics.AnyAsync(bs => bs.BookId == statistic.BookId))
                return Conflict("Statistic for this book already exists.");

            _context.BookStatistics.Add(statistic);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetBookStatistic), new { bookId = statistic.BookId }, statistic);
        }

        // DELETE: api/bookstatistics/5
        [HttpDelete("{bookId}")]
        public async Task<IActionResult> DeleteBookStatistic(int bookId)
        {
            var statistic = await _context.BookStatistics.FindAsync(bookId);
            if (statistic == null)
                return NotFound();

            _context.BookStatistics.Remove(statistic);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
