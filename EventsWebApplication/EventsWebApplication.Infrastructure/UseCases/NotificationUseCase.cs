//Добавлены отдельные use cases для каждой модели

using EventsWebApplication.Core.Interfaces;
using EventsWebApplication.Core.Interfaces.UseCases;
using EventsWebApplication.Core.Models;

namespace EventsWebApplication.Infrastructure.UseCases
{
    public class NotificationUseCase : INotificationUseCase
    {
        private readonly IUnitOfWork _unitOfWork;

        public NotificationUseCase(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Notification>> GetNotificationsForUserAsync(int userId)
        {
            var user = await _unitOfWork.Users.GetUserByIdAsync(userId);
            if (user == null || user.ParticipantId == null)
            {
                throw new UnauthorizedAccessException("Пользователь не зарегистрирован как участник.");
            }

            var notifications = await _unitOfWork.Notifications.GetNotificationsByParticipantIdAsync((int)user.ParticipantId);
            return notifications;
        }
    }
}
