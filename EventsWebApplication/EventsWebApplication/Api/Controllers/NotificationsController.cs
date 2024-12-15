using EventsWebApplication.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventsWebApplication.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NotificationsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public NotificationsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        [Authorize(Policy = "UserPolicy")]
        public async Task<IActionResult> GetNotifications()
        {
            var userId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "id")?.Value ?? "0");
            var user = await _unitOfWork.Users.GetUserByIdAsync(userId);
            if (user == null || user.ParticipantId == null)
            {
                return Unauthorized("Пользователь не зарегистрирован как участник.");
            }

            var notifications = await _unitOfWork.Notifications.GetNotificationsByParticipantIdAsync((int)user.ParticipantId);
            return Ok(notifications);
        }
    }
}
