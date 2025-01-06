using EventsWebApplication.Core.DTOs;

namespace EventsWebApplication.Core.Interfaces.UseCases
{
    public interface IUserUseCase
    {
        Task<string> RegisterUserAsync(UserRegisterDto userDto);
        Task<object> LoginUserAsync(UserLoginDto userDto);
        Task<object> RefreshTokenAsync(string refreshToken);
    }
}
