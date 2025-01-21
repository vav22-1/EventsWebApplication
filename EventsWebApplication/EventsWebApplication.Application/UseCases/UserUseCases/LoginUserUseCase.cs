using EventsWebApplication.Application.DTOs.UserDTOs;
using EventsWebApplication.Core.Interfaces.Services;

namespace EventsWebApplication.Application.UseCases
{
    public class LoginUserUseCase : IUseCase<UserLoginDto, object>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITokenService _tokenService;
        private readonly IPasswordService _passwordService;

        public LoginUserUseCase(IUnitOfWork unitOfWork, ITokenService tokenService, IPasswordService passwordService)
        {
            _unitOfWork = unitOfWork;
            _tokenService = tokenService;
            _passwordService = passwordService;
        }

        public async Task<object> ExecuteAsync(UserLoginDto request)
        {
            var user = await _unitOfWork.Users.GetUserByUsernameAsync(request.Username);
            if (user == null || !_passwordService.ValidatePassword(request.Password, user.PasswordSalt, user.Password))
            {
                throw new UnauthorizedAccessException("Неверный логин или пароль");
            }

            var accessToken = _tokenService.GenerateJwtAccessToken(user);
            user.RefreshToken = Guid.NewGuid().ToString();
            user.RefreshTokenExpiry = DateTime.UtcNow.AddHours(12);

            _unitOfWork.Users.Update(user);
            await _unitOfWork.CompleteAsync();

            return new { AccessToken = accessToken, user.RefreshToken, user.ParticipantId, user.Role };
        }
    }
}
