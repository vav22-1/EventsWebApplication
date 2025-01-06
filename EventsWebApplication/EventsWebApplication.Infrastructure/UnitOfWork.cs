using EventsWebApplication.Core.Interfaces;
using EventsWebApplication.Core.Interfaces.Repositories;
using EventsWebApplication.Infrastructure.Data;

namespace EventsWebApplication.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly EventAppDbContext _dbContext;
        public IEventRepository Events { get; }
        public IPartisipantRepository Participants { get; }
        public IUserRepository Users { get; }
        public INotificationRepository Notifications { get; }

        //Репозитории передаются как параметры конструктора UoW вместо того, чтобы создавать их внутри

        public UnitOfWork(EventAppDbContext dbContext,
                          IEventRepository eventRepository,
                          IPartisipantRepository participantRepository,
                          IUserRepository userRepository,
                          INotificationRepository notificationRepository)
        {
            _dbContext = dbContext;
            Events = eventRepository;
            Participants = participantRepository;
            Users = userRepository;
            Notifications = notificationRepository;
        }

        public async Task CompleteAsync()
        {
            await _dbContext.SaveChangesAsync();
        }
    }

}
