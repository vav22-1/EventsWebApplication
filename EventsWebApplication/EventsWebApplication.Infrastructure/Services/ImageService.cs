using EventsWebApplication.Core.Interfaces.Services;
using Microsoft.Extensions.Configuration;

namespace EventsWebApplication.Infrastructure.Services
{
    public class ImageService : IImageService
    {
        private string _imagePath;
        public ImageService(IConfiguration configuration)
        {
            _imagePath = configuration.GetValue<string>("ImageSavePath");
        }

        public async Task<string> SaveImageAsync(byte[] imageData)
        {
            if (imageData == null || imageData.Length == 0)
            {
                return null;
            }

            var fileName = Guid.NewGuid().ToString() + ".jpg";
            var filePath = Path.Combine(_imagePath, fileName);

            await File.WriteAllBytesAsync(filePath, imageData);

            return fileName;
        }

        public string GetImagePath(string fileName)
        {
            return Path.Combine(_imagePath, fileName);
        }

        public byte[] ReadImage(string fileName)
        {
            var filePath = GetImagePath(fileName);
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException("Файл изображения не найден", fileName);
            }

            return File.ReadAllBytes(filePath);
        }
    }
}
