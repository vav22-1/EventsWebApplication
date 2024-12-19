namespace EventsWebApplication.Infrastructure
{
    public class ImageSaver
    {
        private string _imagePath;
        public ImageSaver(IConfiguration configuration)
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
