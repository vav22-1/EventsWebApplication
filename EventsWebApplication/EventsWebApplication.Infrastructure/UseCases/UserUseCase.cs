//Добавлены отдельные use cases для каждой модели

using EventsWebApplication.Core.DTOs;
using EventsWebApplication.Core.Interfaces.UseCases;
using EventsWebApplication.Core.Interfaces;
using EventsWebApplication.Core.Models;
using Microsoft.Extensions.Configuration;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace EventsWebApplication.Infrastructure.UseCases
{
    public class UserUseCase : IUserUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;

        public UserUseCase(IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
        }

        public async Task<string> RegisterUserAsync(UserRegisterDto userDto)
        {
            if (_unitOfWork.Users.UserExists(userDto.Username))
            {
                throw new InvalidOperationException("Пользователь с таким именем уже существует");
            }

            var user = new User
            {
                Username = userDto.Username,
                Role = userDto.Role
            };

            var newParticipant = new Participant
            {
                DateOfRegistration = DateTime.UtcNow
            };

            await _unitOfWork.Participants.AddAsync(newParticipant);
            await _unitOfWork.CompleteAsync();

            user.ParticipantId = newParticipant.Id;
            await _unitOfWork.Users.Register(user, userDto.Password);
            await _unitOfWork.CompleteAsync();

            return "Успешная регистрация";
        }

        public async Task<object> LoginUserAsync(UserLoginDto userDto)
        {
            var user = await _unitOfWork.Users.Login(userDto.Username, userDto.Password);
            if (user == null)
            {
                throw new UnauthorizedAccessException("Неверный логин или пароль");
            }

            var accessToken = GenerateJwtAccessToken(user);
            var refreshToken = await _unitOfWork.Users.SaveRefreshTokenAsync(user);

            return new { AccessToken = accessToken, RefreshToken = refreshToken, user.ParticipantId, user.Role };
        }

        public async Task<object> RefreshTokenAsync(string refreshToken)
        {
            var user = await _unitOfWork.Users.GetUserByRefreshTokenAsync(refreshToken);
            if (user == null || user.RefreshTokenExpiry < DateTime.UtcNow)
            {
                throw new UnauthorizedAccessException("Недействительный refresh токен");
            }

            var newAccessToken = GenerateJwtAccessToken(user);
            var newRefreshToken = await _unitOfWork.Users.SaveRefreshTokenAsync(user);

            await _unitOfWork.CompleteAsync();

            return new { AccessToken = newAccessToken, RefreshToken = newRefreshToken };
        }

        private string GenerateJwtAccessToken(User user)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role),
                new Claim("id", user.Id.ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:SecretKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["JwtSettings:Issuer"],
                audience: _configuration["JwtSettings:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(15),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
