using EventsWebApplication.Application.DTOs;
using EventsWebApplication.Core.Interfaces;
using EventsWebApplication.Core.Models;

namespace EventsWebApplication.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationsController : ControllerBase
    {
        private readonly IUseCase<NotificationRequestDto, IEnumerable<Notification>> _notificationUseCase;

        public NotificationsController(IUseCase<NotificationRequestDto, IEnumerable<Notification>> notificationUseCase)
        {
            _notificationUseCase = notificationUseCase;
        }

        [HttpGet]
        [Authorize(Policy = "UserPolicy")]
        public async Task<IActionResult> GetNotifications()
        {
            var userId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "id")?.Value ?? "0");

            var requestDto = new NotificationRequestDto { UserId = userId };

            var notifications = await _notificationUseCase.ExecuteAsync(requestDto);

            return Ok(notifications);
        }
    }
}
