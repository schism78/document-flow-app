using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using api.Models;
using api.Data;

namespace api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DocumentRoutesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public DocumentRoutesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/DocumentRoutes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DocumentRoute>>> GetDocumentRoutes()
        {
            return await _context.DocumentRoutes
                .Include(dr => dr.Document)
                .Include(dr => dr.FromUser)
                .Include(dr => dr.ToUser)
                .ToListAsync();
        }

        // GET: api/DocumentRoutes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<DocumentRoute>> GetDocumentRoute(int id)
        {
            var documentRoute = await _context.DocumentRoutes
                .Include(dr => dr.Document)
                .Include(dr => dr.FromUser)
                .Include(dr => dr.ToUser)
                .FirstOrDefaultAsync(dr => dr.Id == id);

            if (documentRoute == null)
            {
                return NotFound();
            }

            return documentRoute;
        }

        // POST: api/DocumentRoutes
        [HttpPost]
        public async Task<ActionResult<DocumentRoute>> CreateDocumentRoute(DocumentRoute documentRoute)
        {
            _context.DocumentRoutes.Add(documentRoute);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetDocumentRoute), new { id = documentRoute.Id }, documentRoute);
        }

        // PUT: api/DocumentRoutes/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDocumentRoute(int id, DocumentRoute documentRoute)
        {
            if (id != documentRoute.Id)
            {
                return BadRequest();
            }

            _context.Entry(documentRoute).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DocumentRouteExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/DocumentRoutes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDocumentRoute(int id)
        {
            var documentRoute = await _context.DocumentRoutes.FindAsync(id);
            if (documentRoute == null)
            {
                return NotFound();
            }

            _context.DocumentRoutes.Remove(documentRoute);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool DocumentRouteExists(int id)
        {
            return _context.DocumentRoutes.Any(e => e.Id == id);
        }
    }
}
