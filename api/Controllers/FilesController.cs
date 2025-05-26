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
        public async Task<IActionResult> UploadFile([FromForm] IFormFile file)
        {
            Console.WriteLine("UploadFile method called."); // Логируем вызов метода

            if (file == null || file.Length == 0)
            {
                Console.WriteLine("File is null or empty."); // Логируем ошибку
                return BadRequest("File is empty or not provided.");
            }

            try
            {
                using var stream = file.OpenReadStream();
                Console.WriteLine($"Uploading file: {file.FileName}, size: {file.Length} bytes"); // Логируем размер и имя файла

                await _storageService.UploadFileAsync(file.FileName, stream);

                Console.WriteLine("File uploaded successfully."); // Логируем успешную загрузку
                return Ok("File uploaded successfully");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during file upload: {ex.Message}");
                Console.WriteLine(ex.StackTrace);
                if (ex.InnerException != null)
                    Console.WriteLine($"Inner: {ex.InnerException.Message}");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
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