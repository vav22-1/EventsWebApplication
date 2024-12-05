using EventsWebApplication.Core.Interfaces;
using EventsWebApplication.Infrastructure.Repositories;
using EventsWebApplication.Infrasturture.Data;

namespace EventsWebApplication.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private EventAppDbContext _dbContext;
        public IEventRepository Events {  get; set; }
        public IPartisipantRepository Participants {  get; set; }

        public UnitOfWork(EventAppDbContext dbContext)
        {
            _dbContext = dbContext;
            Events = new EventRepository(dbContext);
            Participants = new ParticipantRepository(dbContext);
        }
        public async Task CompleteAsync()
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}
