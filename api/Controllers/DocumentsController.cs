using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using api.Data;
using api.Models;

namespace api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DocumentsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public DocumentsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/documents
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Document>>> GetDocuments()
        {
            return await _context.Documents
                .Include(d => d.SenderUser)
                .Include(d => d.CurrentUser)
                .ToListAsync();
        }

        // GET: api/documents/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Document>> GetDocument(int id)
        {
            var document = await _context.Documents
                .Include(d => d.SenderUser)
                .Include(d => d.CurrentUser)
                .FirstOrDefaultAsync(d => d.Id == id);

            if (document == null)
            {
                return NotFound();
            }

            return document;
        }

        // POST: api/documents
        [HttpPost]
        public async Task<ActionResult<Document>> CreateDocument(Document document)
        {
            document.CreatedAt = DateTime.UtcNow;

            _context.Documents.Add(document);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetDocument), new { id = document.Id }, document);
        }

        // PUT: api/documents/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDocument(int id, Document updatedDocument)
        {
            if (id != updatedDocument.Id)
                return BadRequest();

            var existingDoc = await _context.Documents.FindAsync(id);
            if (existingDoc == null)
                return NotFound();

            existingDoc.Title = updatedDocument.Title;
            existingDoc.Status = updatedDocument.Status;
            existingDoc.CurrentUserId = updatedDocument.CurrentUserId;

            _context.Entry(existingDoc).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/documents/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDocument(int id)
        {
            var document = await _context.Documents.FindAsync(id);
            if (document == null)
                return NotFound();

            _context.Documents.Remove(document);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
