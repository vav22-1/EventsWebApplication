//Хеширование пароля и генерацию соли, проверку валидности пароля вынесены в отдельный сервис 

using EventsWebApplication.Core.Interfaces.Services;
using System.Security.Cryptography;
using System.Text;

namespace EventsWebApplication.Infrastructure.Services
{
    public class PasswordService : IPasswordService
    {
        public byte[] HashPassword(string password, byte[] salt)
        {
            using var hmac = new HMACSHA512(salt);
            return hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        }

        public byte[] GenerateSalt()
        {
            using var hmac = new HMACSHA512();
            return hmac.Key;
        }

        public bool ValidatePassword(string password, byte[] salt, byte[] hashedPassword)
        {
            var computedHash = HashPassword(password, salt);
            return computedHash.SequenceEqual(hashedPassword);
        }
    }
}