using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using api.Data;
using api.Models;

namespace api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuditLogsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AuditLogsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/auditlogs
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AuditLog>>> GetAuditLogs()
        {
            var logs = await _context.AuditLogs
                .Include(log => log.User)
                .OrderByDescending(log => log.Timestamp)
                .ToListAsync();

            return Ok(logs);
        }

        // GET: api/auditlogs/user/5
        [HttpGet("user/{userId}")]
        public async Task<ActionResult<IEnumerable<AuditLog>>> GetAuditLogsByUser(int userId)
        {
            var logs = await _context.AuditLogs
                .Include(log => log.User)
                .Where(log => log.UserId == userId)
                .OrderByDescending(log => log.Timestamp)
                .ToListAsync();

            return Ok(logs);
        }

        // GET: api/auditlogs/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AuditLog>> GetAuditLog(int id)
        {
            var log = await _context.AuditLogs
                .Include(log => log.User)
                .FirstOrDefaultAsync(log => log.Id == id);

            if (log == null)
                return NotFound();

            return Ok(log);
        }

        // POST: api/auditlogs
        [HttpPost]
        public async Task<ActionResult<AuditLog>> CreateAuditLog(AuditLog auditLog)
        {
            auditLog.Timestamp = DateTime.UtcNow;

            _context.AuditLogs.Add(auditLog);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetAuditLog), new { id = auditLog.Id }, auditLog);
        }
    }
}
