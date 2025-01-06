using EventsWebApplication.Core.DTOs;
using EventsWebApplication.Core.Interfaces.UseCases;

namespace EventsWebApplication.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserUseCase _userUseCase;

        public UserController(IUserUseCase userUseCase)
        {
            _userUseCase = userUseCase;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserRegisterDto userDto)
        {
            var result = await _userUseCase.RegisterUserAsync(userDto);
            return Ok(result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserLoginDto userDto)
        {
            var result = await _userUseCase.LoginUserAsync(userDto);
            return Ok(result);
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> RefreshToken([FromBody] string refreshToken)
        {
            var result = await _userUseCase.RefreshTokenAsync(refreshToken);
            return Ok(result);
        }
    }
}
