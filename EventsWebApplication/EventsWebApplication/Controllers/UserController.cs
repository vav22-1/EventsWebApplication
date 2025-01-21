using EventsWebApplication.Application.DTOs.UserDTOs;
using EventsWebApplication.Core.Interfaces;

namespace EventsWebApplication.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUseCase<UserRegisterDto, string> _registerUserUseCase;
        private readonly IUseCase<UserLoginDto, object> _loginUserUseCase;
        private readonly IUseCase<RefreshTokenRequestDto, object> _refreshTokenUseCase;

        public UserController(
            IUseCase<UserRegisterDto, string> registerUserUseCase,
            IUseCase<UserLoginDto, object> loginUserUseCase,
            IUseCase<RefreshTokenRequestDto, object> refreshTokenUseCase)
        {
            _registerUserUseCase = registerUserUseCase;
            _loginUserUseCase = loginUserUseCase;
            _refreshTokenUseCase = refreshTokenUseCase;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegisterDto userDto)
        {
            var result = await _registerUserUseCase.ExecuteAsync(userDto);
            return Ok(result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDto userDto)
        {
            var result = await _loginUserUseCase.ExecuteAsync(userDto);
            return Ok(result);
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> RefreshToken([FromBody] string refreshToken)
        {
            var refreshDto = new RefreshTokenRequestDto { RefreshToken = refreshToken };
            var result = await _refreshTokenUseCase.ExecuteAsync(refreshDto);
            return Ok(result);
        }
    }
}
