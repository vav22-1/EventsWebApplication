using EventsWebApplication.Core.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace EventsWebApplication.Api.Middleware
{
    public class RoleMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IConfiguration _configuration;

        public RoleMiddleware(RequestDelegate next, IConfiguration configuration)
        {
            _next = next;
            _configuration = configuration;
        }

        public async Task Invoke(HttpContext context)
        {
            var token = context.Request.Headers.Authorization.FirstOrDefault()?.Split(" ").Last();
            if (token != null)
            {
                try
                {
                    // Настроим параметры для валидации токена
                    var tokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true, // Проверка срока действия
                        ValidateIssuerSigningKey = true, // Проверка подписи
                        ValidIssuer = _configuration["JwtSettings:Issuer"],
                        ValidAudience = _configuration["JwtSettings:Audience"],
                        ClockSkew = TimeSpan.Zero,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:SecretKey"]))
                    };

                    var tokenHandler = new JwtSecurityTokenHandler();
                    var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var validatedToken);

                    // Если токен был успешно валидирован, устанавливаем его как User
                    context.User = principal;
                }
                catch (Exception ex)
                {
                    // Ошибка валидации токена
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    await context.Response.WriteAsync($"Unauthorized: {ex.Message}");
                    return;
                }
            }

            // Если токен отсутствует, пропускаем запрос дальше
            await _next(context);
        }
    }
}
