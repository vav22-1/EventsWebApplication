using EventsWebApplication.Core.Models;
namespace EventsWebApplication.Core.Interfaces.UseCases
{
    public interface INotificationUseCase
    {
        Task<IEnumerable<Notification>> GetNotificationsForUserAsync(int userId);
    }
}
