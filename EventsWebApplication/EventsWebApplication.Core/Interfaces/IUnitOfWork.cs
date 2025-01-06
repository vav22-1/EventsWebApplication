using EventsWebApplication.Core.Interfaces.Repositories;

namespace EventsWebApplication.Core.Interfaces
{
    public interface IUnitOfWork
    {
        IEventRepository Events { get; }
        IPartisipantRepository Participants { get; }
        IUserRepository Users { get; }
        INotificationRepository Notifications { get; }
        Task CompleteAsync();
    }
}
