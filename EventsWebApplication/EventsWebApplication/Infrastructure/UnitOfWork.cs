using EventsWebApplication.Core.Interfaces;
using EventsWebApplication.Infrastructure.Repositories;
using EventsWebApplication.Infrastructure.Data;

namespace EventsWebApplication.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly EventAppDbContext _dbContext;
        public IEventRepository Events { get; set; }
        public IPartisipantRepository Participants { get; set; }
        public IUserRepository Users { get; set; }
        public INotificationRepository Notifications { get; set; }

        public UnitOfWork(EventAppDbContext dbContext)
        {
            _dbContext = dbContext;
            Events = new EventRepository(dbContext);
            Participants = new ParticipantRepository(dbContext);
            Users = new UserRepository(dbContext);
            Notifications = new NotificationRepository(dbContext);
        }
        public async Task CompleteAsync()
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}
