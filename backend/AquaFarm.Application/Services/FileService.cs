using AquaFarm.Application.CustomExceptions;
using AquaFarm.Application.Services.Contracts;
using Microsoft.Extensions.Configuration;

namespace AquaFarm.Application.Services
{
    public class FileService : IFileService
    {
        private readonly string _basePath;

        public FileService(IConfiguration config)
        {
            _basePath = config["FileStorage:BasePath"] ?? Path.Combine("wwwroot", "images");
        }

        public async Task<string> UploadImageAsync(Stream content, string? fileName = null)
        {
            try
            {
                if (!Directory.Exists(_basePath))
                {
                    Directory.CreateDirectory(_basePath);
                }

                var extension = fileName != null ? Path.GetExtension(fileName) : ".jpg";
                var uniqueFileName = $"{Guid.NewGuid()}{extension}";
                var filePath = Path.Combine(_basePath, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await content.CopyToAsync(fileStream);
                }

                return $"/images/{uniqueFileName}";
            }
            catch (Exception ex)
            {
                throw new FileUploadException(
                    action: "UPLOAD_IMAGE",
                    statusCode: 500,
                    message: ex.Message,
                    innerException: ex);
            }
        }
    }
}
