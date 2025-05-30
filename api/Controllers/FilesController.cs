    using Microsoft.AspNetCore.Mvc;
    using YandexCloudStorageApp.Services;
    using System.Threading.Tasks;

    namespace YandexCloudStorageApp.Controllers
    {
    [ApiController]
    [Route("api/[controller]")]
    public class FilesController : ControllerBase
    {
        private readonly YandexStorageService _storageService;

        public FilesController(YandexStorageService storageService)
        {
            _storageService = storageService;
        }

        // Загрузка файла
        [HttpPost("upload")]
        public async Task<IActionResult> UploadFiles([FromForm] List<IFormFile> files)
        {
            if (files == null || files.Count == 0)
            {
                return BadRequest("No files provided.");
            }

            var uploadedFiles = new List<string>();

            foreach (var file in files)
            {
                try
                {
                    using var stream = file.OpenReadStream();
                    await _storageService.UploadFileAsync(file.FileName, stream);
                    uploadedFiles.Add(file.FileName);
                }
                catch (Exception ex)
                {
                    return StatusCode(500, $"Error uploading {file.FileName}: {ex.Message}");
                }
            }

            return Ok(new { Message = "Files uploaded successfully", Files = uploadedFiles });
        }

        // Получение файла
        [HttpGet("download/{fileName}")]
        public async Task<IActionResult> DownloadFile(string fileName)
        {
            var stream = await _storageService.GetFileAsync(fileName);
            return File(stream, "application/octet-stream", fileName);
        }

        // Удаление файла
        [HttpDelete("delete/{fileName}")]
        public async Task<IActionResult> DeleteFile(string fileName)
        {
            await _storageService.DeleteFileAsync(fileName);
            return Ok("File deleted successfully");
        }
            
            [HttpGet("test")]
            public IActionResult Test()
            {
                return Ok("Files controller is running!");
            }
        }
    }