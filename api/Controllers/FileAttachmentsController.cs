using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using api.Models;
using api.Data;
using YandexCloudStorageApp.Services;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using System.Linq;

namespace api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FileAttachmentsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly YandexStorageService _storageService;

        public FileAttachmentsController(AppDbContext context, YandexStorageService storageService)
        {
            _context = context;
            _storageService = storageService;
        }

        // Получить файл по ID
        [HttpGet("{id}")]
        public async Task<ActionResult<FileAttachment>> GetFileAttachment(int id)
        {
            var fileAttachment = await _context.FileAttachments.FindAsync(id);
            if (fileAttachment == null)
                return NotFound();

            return fileAttachment;
        }

        // Получить все файлы по книге
        [HttpGet("ByBook/{bookId}")]
        public async Task<ActionResult<IEnumerable<FileAttachment>>> GetFilesByBook(int bookId)
        {
            var files = await _context.FileAttachments
                .Where(f => f.BookId == bookId)
                .ToListAsync();

            return files;
        }

        // Загрузить файл с привязкой к книге
        /*
        [HttpPost]
        public async Task<ActionResult<FileAttachment>> UploadFile([FromForm] IFormFile file, [FromForm] int bookId)
        {
            if (file == null || file.Length == 0)
                return BadRequest("Файл не загружен");

            var book = await _context.Books.FindAsync(bookId);
            if (book == null)
                return BadRequest("Книга не найдена");

            // Формируем ключ для файла в хранилище
            var key = $"{bookId}/{file.FileName}";

            using (var stream = file.OpenReadStream())
            {
                await _storageService.UploadFileAsync(key, stream);
            }

            var fileAttachment = new FileAttachment
            {
                BookId = bookId,
                FileName = file.FileName,
                Url = _storageService.GetFileUrl(key)

            };

            _context.FileAttachments.Add(fileAttachment);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetFileAttachment), new { id = fileAttachment.Id }, fileAttachment);
        }

        НЕ РАБОТАЕТ С SWAGGER

        */

        // Удалить файл и запись
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFileAttachment(int id)
        {
            var fileAttachment = await _context.FileAttachments.FindAsync(id);
            if (fileAttachment == null)
                return NotFound();

            var key = $"{fileAttachment.BookId}/{fileAttachment.FileName}";
            await _storageService.DeleteFileAsync(key);

            _context.FileAttachments.Remove(fileAttachment);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
