using EventsWebApplication.Core.Models;

namespace EventsWebApplication.Core.Interfaces.Repositories
{
    public interface IUserRepository : IRepository<User>
    {
        bool UserExists(string username);
        Task<User> GetUserByRefreshTokenAsync(string refreshToken);
        Task<User> GetUserByUsernameAsync(string username);
    }
}
