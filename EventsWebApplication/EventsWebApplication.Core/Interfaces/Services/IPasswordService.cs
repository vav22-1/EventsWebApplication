namespace EventsWebApplication.Core.Interfaces.Services
{
    public interface IPasswordService
    {
        byte[] HashPassword(string password, byte[] salt);
        byte[] GenerateSalt();
        bool ValidatePassword(string password, byte[] salt, byte[] hashedPassword);
    }
}
