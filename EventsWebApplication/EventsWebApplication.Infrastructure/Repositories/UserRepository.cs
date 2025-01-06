using EventsWebApplication.Core.Interfaces.Repositories;
using EventsWebApplication.Core.Interfaces.Services;
using EventsWebApplication.Core.Models;
using EventsWebApplication.Infrastructure.Data;

namespace EventsWebApplication.Infrastructure.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        private readonly IPasswordService _passwordService;

        public UserRepository(EventAppDbContext dbContext, IPasswordService passwordService) : base(dbContext)
        {
            _passwordService = passwordService;
        }

        public async Task<User> Register(User user, string password)
        {
            user.PasswordSalt = _passwordService.GenerateSalt();
            user.Password = _passwordService.HashPassword(password, user.PasswordSalt);

            _dbContext.Users.Add(user);
            return user;
        }

        public async Task<User> Login(string username, string password)
        {
            var user = await _dbContext.Users.SingleOrDefaultAsync(x => x.Username == username);
            if (user == null) return null;

            var isValidPassword = _passwordService.ValidatePassword(password, user.PasswordSalt, user.Password);
            return isValidPassword ? user : null;
        }

        public bool UserExists(string username)
        {
            return _dbContext.Users.Any(x => x.Username == username);
        }

        public async Task<string> SaveRefreshTokenAsync(User user)
        {
            user.RefreshToken = Guid.NewGuid().ToString();
            user.RefreshTokenExpiry = DateTime.UtcNow.AddHours(12);
            _dbContext.Users.Update(user);
            return user.RefreshToken;
        }

        public async Task<User> GetUserByRefreshTokenAsync(string refreshToken)
        {
            return await _dbContext.Users.SingleOrDefaultAsync(u => u.RefreshToken == refreshToken);
        }

        public async Task<User> GetUserByIdAsync(int id)
        {
            return await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == id);
        }
    }
}
