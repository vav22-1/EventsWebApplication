using EventsWebApplication.Core.Interfaces.UseCases;

namespace EventsWebApplication.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NotificationsController : ControllerBase
    {
        private readonly INotificationUseCase _notificationUseCase;

        public NotificationsController(INotificationUseCase notificationUseCase)
        {
            _notificationUseCase = notificationUseCase;
        }

        [HttpGet]
        [Authorize(Policy = "UserPolicy")]
        public async Task<IActionResult> GetNotifications()
        {
            var userId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "id")?.Value ?? "0");
            var notifications = await _notificationUseCase.GetNotificationsForUserAsync(userId);
            return Ok(notifications);
        }
    }
}
