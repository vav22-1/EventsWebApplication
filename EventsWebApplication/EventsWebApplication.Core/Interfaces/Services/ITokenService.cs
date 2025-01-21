using EventsWebApplication.Core.Models;

namespace EventsWebApplication.Core.Interfaces.Services
{
    public interface ITokenService
    {
        string GenerateJwtAccessToken(User user);
    }
}
