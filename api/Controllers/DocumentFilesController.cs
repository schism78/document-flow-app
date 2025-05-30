using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using api.Data;
using api.Models;

namespace api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DocumentFilesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public DocumentFilesController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<ActionResult<DocumentFile>> AddFile(DocumentFile file)
        {
            _context.DocumentFiles.Add(file);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetFile), new { id = file.Id }, file);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<DocumentFile>> GetFile(int id)
        {
            var file = await _context.DocumentFiles
                .Include(f => f.Document)
                .FirstOrDefaultAsync(f => f.Id == id);

            if (file == null)
                return NotFound();

            return file;
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFile(int id)
        {
            var file = await _context.DocumentFiles.FindAsync(id);
            if (file == null)
                return NotFound();

            _context.DocumentFiles.Remove(file);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}