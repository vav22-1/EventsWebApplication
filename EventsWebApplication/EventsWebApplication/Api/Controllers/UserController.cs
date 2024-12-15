using Microsoft.AspNetCore.Mvc;
using EventsWebApplication.Core.DTOs;
using AutoMapper;
using EventsWebApplication.Core.Models;
using Microsoft.AspNetCore.Identity;
using EventsWebApplication.Core.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Azure.Core;

namespace EventsWebApplication.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        public UserController(IUnitOfWork unitOfWork, IConfiguration configuration, IMapper mapper)
        {
            _unitOfWork = unitOfWork;   
            _configuration = configuration;
            _mapper = mapper;
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register(UserRegisterDto userDto)
        {
            if (_unitOfWork.Users.UserExists(userDto.Username))
            {
                return BadRequest("Пользователь с таким именем уже существует");
            }
            var user = _mapper.Map<User>(userDto);
            var newParticipant = new Participant();
            await _unitOfWork.Participants.AddParticipantAsync(newParticipant);
            await _unitOfWork.CompleteAsync();
            user.ParticipantId = newParticipant.Id;
            await _unitOfWork.Users.Register(user, userDto.Password);
            return Ok("Успешная регистрация");
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login(UserLoginDto userDto)
        {
            var user = await _unitOfWork.Users.Login(userDto.Username, userDto.Password);
            if (user == null) return Unauthorized("Неверный логин или пароль");
            var accessToken = GenerateJwtAccessToken(user);
            var refreshToken = await _unitOfWork.Users.SaveRefreshTokenAsync(user);
            return Ok(new { AccessToken = accessToken, RefreshToken = refreshToken, user.ParticipantId, user.Role });
        }
        [HttpPost("refresh")]
        public async Task<IActionResult> RefreshToken([FromBody] string refreshToken)
        {
            var user = await _unitOfWork.Users.GetUserByRefreshTokenAsync(refreshToken);
            if (user == null || user.RefreshTokenExpiry < DateTime.UtcNow)
            {
                return Unauthorized("Недействительный refresh токен");
            }

            var newAccessToken = GenerateJwtAccessToken(user);
            var newRefreshToken = await _unitOfWork.Users.SaveRefreshTokenAsync(user);

            return Ok(new { AccessToken = newAccessToken, RefreshToken = newRefreshToken });
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
