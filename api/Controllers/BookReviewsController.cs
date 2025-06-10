using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using api.Data;
using api.Models;

namespace api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookReviewsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public BookReviewsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/bookreviews
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookReview>>> GetBookReviews()
        {
            var reviews = await _context.BookReviews
                .Include(r => r.User)
                .Include(r => r.Book)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();

            return Ok(reviews);
        }

        // GET: api/bookreviews/book/5
        [HttpGet("book/{bookId}")]
        public async Task<ActionResult<IEnumerable<BookReview>>> GetReviewsByBook(int bookId)
        {
            var reviews = await _context.BookReviews
                .Include(r => r.User)
                .Where(r => r.BookId == bookId)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();

            return Ok(reviews);
        }

        // GET: api/bookreviews/5
        [HttpGet("{id}")]
        public async Task<ActionResult<BookReview>> GetBookReview(int id)
        {
            var review = await _context.BookReviews
                .Include(r => r.User)
                .Include(r => r.Book)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (review == null)
                return NotFound();

            return Ok(review);
        }

        // POST: api/bookreviews
        [HttpPost]
        public async Task<ActionResult<BookReview>> CreateBookReview(BookReview review)
        {
            review.CreatedAt = DateTime.UtcNow;

            _context.BookReviews.Add(review);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetBookReview), new { id = review.Id }, review);
        }

        // PUT: api/bookreviews/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBookReview(int id, BookReview updatedReview)
        {
            if (id != updatedReview.Id)
                return BadRequest();

            var existingReview = await _context.BookReviews.FindAsync(id);
            if (existingReview == null)
                return NotFound();

            existingReview.Text = updatedReview.Text;
            existingReview.Rating = updatedReview.Rating;
            // Не меняем UserId и BookId
            // Можно обновить CreatedAt если нужно (например дата правки)

            _context.Entry(existingReview).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/bookreviews/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBookReview(int id)
        {
            var review = await _context.BookReviews.FindAsync(id);
            if (review == null)
                return NotFound();

            _context.BookReviews.Remove(review);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
