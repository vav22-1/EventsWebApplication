using EventsWebApplication.Core.Interfaces.Repositories;
using EventsWebApplication.Core.Models;
using EventsWebApplication.Infrastructure.Data;
using EventsWebApplication.Infrastructure.Services;

namespace EventsWebApplication.Infrastructure.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {

        public UserRepository(EventAppDbContext dbContext) : base(dbContext) { }

        public bool UserExists(string username)
        {
            return _dbContext.Users.Any(x => x.Username == username);
        }

        public async Task<User> GetUserByRefreshTokenAsync(string refreshToken)
        {
            return await _dbContext.Users.SingleOrDefaultAsync(u => u.RefreshToken == refreshToken);
        }

        public async Task<User> GetUserByUsernameAsync(string username)
        {
            return await _dbContext.Users.SingleOrDefaultAsync(x => x.Username == username);
        }
    }
}
