namespace EventsWebApplication.Core.Interfaces.Services
{
    public interface IImageService
    {
        Task<string> SaveImageAsync(byte[] imageData);

        string GetImagePath(string fileName);

        byte[] ReadImage(string fileName);
    }
}
