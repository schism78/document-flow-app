using Amazon.S3;
using Amazon.S3.Model;
using DotNetEnv;
using System.IO;
using System.Threading.Tasks;

namespace YandexCloudStorageApp.Services
{
    public class YandexStorageService
    {
        private readonly IAmazonS3 _s3Client;
        private readonly string _bucketName;

        public YandexStorageService(IAmazonS3 s3Client)
        {
            _s3Client = s3Client;
            _bucketName = Environment.GetEnvironmentVariable("BUCKET_NAME");
        }

        public string GetFileUrl(string key)
        {
            return $"https://{_bucketName}.storage.yandexcloud.net/{key}";
        }

        // Загрузка файла
        public async Task UploadFileAsync(string key, Stream fileStream)
        {
            var request = new PutObjectRequest
            {
                BucketName = _bucketName,
                Key = key,
                InputStream = fileStream
            };

            await _s3Client.PutObjectAsync(request);
        }

        // Получение файла
        public async Task<Stream> GetFileAsync(string key)
        {
            var request = new GetObjectRequest
            {
                BucketName = _bucketName,
                Key = key
            };

            var response = await _s3Client.GetObjectAsync(request);
            return response.ResponseStream;
        }

        // Удаление файла
        public async Task DeleteFileAsync(string key)
        {
            var request = new DeleteObjectRequest
            {
                BucketName = _bucketName,
                Key = key
            };

            await _s3Client.DeleteObjectAsync(request);
        }
    }
}