using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace EventsWebApplication.Infrastructure.Services
{
    public class ImageService
    {
        private string _imagePath;
        public ImageService(IConfiguration configuration)
        {
            _imagePath = configuration.GetValue<string>("ImageSavePath");
        }
        public async Task<string> SaveImageAsync(IFormFile image)
        {
            if (image == null)
            {
                return null;
            }
            var fileName = Path.GetFileNameWithoutExtension(image.FileName) + "_" + Guid.NewGuid() + Path.GetExtension(image.FileName);
            var filePath = Path.Combine(_imagePath, fileName);

            using (var imageStream = new FileStream(filePath, FileMode.Create))
            {
                await image.CopyToAsync(imageStream);
            }
            return fileName;
        }
        public string GetImagePath(string fileName)
        {
            return Path.Combine(_imagePath, fileName);
        }
    }
}
