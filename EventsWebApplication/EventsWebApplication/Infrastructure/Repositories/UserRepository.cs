using EventsWebApplication.Core.Interfaces;
using EventsWebApplication.Core.Models;
using EventsWebApplication.Infrastructure.Data;
using System.Security.Cryptography;
using System.Text;

namespace EventsWebApplication.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly EventAppDbContext _dbContext;

        public UserRepository(EventAppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<User> Register(User user, string password)
        {
            using var hmac = new HMACSHA512();
            user.Password = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            user.PasswordSalt = hmac.Key;

            _dbContext.Users.Add(user);
            await _dbContext.SaveChangesAsync();
            return user;
        }
        public async Task<User> Login(string username, string password)
        {
            var user = await _dbContext.Users.SingleOrDefaultAsync(x => x.Username == username);
            if (user == null) return null;
            using var hmac = new HMACSHA512(user.PasswordSalt);
            var hashedPassword = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            if (!hashedPassword.SequenceEqual(user.Password)) return null;
            return user;
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
            await _dbContext.SaveChangesAsync();
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
