using EventsWebApplication.Application.DTOs;
using EventsWebApplication.Core.Models;

namespace EventsWebApplication.Application.UseCases
{
    public class NotificationUseCase : IUseCase<NotificationRequestDto, IEnumerable<Notification>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public NotificationUseCase(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Notification>> ExecuteAsync(NotificationRequestDto request)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(request.UserId);
            if (user == null || user.ParticipantId == null)
            {
                throw new UnauthorizedAccessException("Пользователь не зарегистрирован как участник.");
            }

            var notifications = await _unitOfWork.Notifications.GetNotificationsByParticipantIdAsync((int)user.ParticipantId);
            return notifications;
        }
    }
}
