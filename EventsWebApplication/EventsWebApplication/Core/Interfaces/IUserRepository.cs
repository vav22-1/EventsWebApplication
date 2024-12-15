using EventsWebApplication.Core.Models;

namespace EventsWebApplication.Core.Interfaces
{
    public interface IUserRepository
    {
        Task<User> Register(User user, string password);
        Task<User> Login(string username, string password);
        bool UserExists(string username);
        Task<string> SaveRefreshTokenAsync(User user);
        Task<User> GetUserByRefreshTokenAsync(string refreshToken);
        Task<User> GetUserByIdAsync(int id);
    }
}
