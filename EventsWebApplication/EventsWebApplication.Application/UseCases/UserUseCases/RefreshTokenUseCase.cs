using EventsWebApplication.Core.DTOs.UserDTOs;
using EventsWebApplication.Core.Interfaces;
using EventsWebApplication.Core.Interfaces.Services;

namespace EventsWebApplication.Application.UseCases
{
    public class RefreshTokenUseCase : IUseCase<RefreshTokenRequestDto, object>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITokenService _tokenService;

        public RefreshTokenUseCase(IUnitOfWork unitOfWork, ITokenService tokenService)
        {
            _unitOfWork = unitOfWork;
            _tokenService = tokenService;
        }

        public async Task<object> ExecuteAsync(RefreshTokenRequestDto request)
        {
            var user = await _unitOfWork.Users.GetUserByRefreshTokenAsync(request.RefreshToken);
            if (user == null || user.RefreshTokenExpiry < DateTime.UtcNow)
            {
                throw new UnauthorizedAccessException("Недействительный refresh токен");
            }

            var newAccessToken = _tokenService.GenerateJwtAccessToken(user);

            user.RefreshToken = Guid.NewGuid().ToString();
            user.RefreshTokenExpiry = DateTime.UtcNow.AddHours(12);

            _unitOfWork.Users.Update(user);
            await _unitOfWork.CompleteAsync();

            return new { AccessToken = newAccessToken, user.RefreshToken };
        }
    }
}
