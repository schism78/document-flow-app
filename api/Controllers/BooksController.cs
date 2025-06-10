using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using api.Data;
using api.Models;

namespace api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BooksController : ControllerBase
    {
        private readonly AppDbContext _context;

        public BooksController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/books
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Book>>> GetBooks()
        {
            return await _context.Books
                .Include(b => b.Genre)
                .Include(b => b.Reviews)
                .Include(b => b.FileAttachments)
                .ToListAsync();
        }

        // GET: api/books/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Book>> GetBook(int id)
        {
            var book = await _context.Books
                .Include(b => b.Genre)
                .Include(b => b.Reviews)
                .Include(b => b.FileAttachments)
                .FirstOrDefaultAsync(b => b.Id == id);

            if (book == null)
                return NotFound();

            return book;
        }

        // POST: api/books
        [HttpPost]
        public async Task<ActionResult<Book>> CreateBook(Book book)
        {
            // Изначально доступно столько же, сколько всего
            book.AvailableCopies = book.TotalCopies;

            _context.Books.Add(book);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetBook), new { id = book.Id }, book);
        }

        // PUT: api/books/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBook(int id, Book updatedBook)
        {
            if (id != updatedBook.Id)
                return BadRequest();

            var book = await _context.Books.FindAsync(id);
            if (book == null)
                return NotFound();

            // Обновляем только основные поля (без жёсткой логики для копий)
            book.Title = updatedBook.Title;
            book.Author = updatedBook.Author;
            book.Annotation = updatedBook.Annotation;
            book.GenreId = updatedBook.GenreId;

            // Обновление количества книг
            int difference = updatedBook.TotalCopies - book.TotalCopies;
            book.TotalCopies = updatedBook.TotalCopies;
            book.AvailableCopies += difference;

            // Защита от отрицательного количества
            if (book.AvailableCopies < 0)
                book.AvailableCopies = 0;

            _context.Entry(book).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/books/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book == null)
                return NotFound();

            _context.Books.Remove(book);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
