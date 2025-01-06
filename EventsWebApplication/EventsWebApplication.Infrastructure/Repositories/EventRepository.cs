using EventsWebApplication.Core.DTOs;
using EventsWebApplication.Core.Interfaces.Repositories;
using EventsWebApplication.Core.Models;
using EventsWebApplication.Infrastructure.Data;

namespace EventsWebApplication.Infrastructure.Repositories
{
    public class EventRepository : Repository<Event>, IEventRepository
    {
        public EventRepository(EventAppDbContext dbContext) : base(dbContext) { }

        public async Task<Event> GetEventByIdAsync(int id)
        {
            return await _dbContext.Events
                .Include(e => e.EventParticipants)
                .ThenInclude(ep => ep.Participant)
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<Event> GetEventByTitleAsync(string title)
        {
            return await _dbContext.Events
                .Include(e => e.EventParticipants)
                .ThenInclude(ep => ep.Participant)
                .FirstOrDefaultAsync(e => e.Title == title);
        }

        public IQueryable<Event> GetEventsWithFilterQuery(EventFilterDto filter)
        {
            var query = _dbContext.Events.AsQueryable();

            if (!string.IsNullOrWhiteSpace(filter.Title))
            {
                query = query.Where(e => e.Title.StartsWith(filter.Title));
            }
            if (filter.FilterDate.HasValue)
            {
                query = query.Where(e => e.DateAndTime.Date == filter.FilterDate.Value.Date);
            }
            if (!string.IsNullOrWhiteSpace(filter.FilterLocation))
            {
                query = query.Where(e => e.Location.StartsWith(filter.FilterLocation));
            }
            if (!string.IsNullOrWhiteSpace(filter.FilterCategory))
            {
                query = query.Where(e => e.Category.StartsWith(filter.FilterCategory));
            }

            return query;
        }

        public async Task<int> GetCurrentParticipantsAsync(int eventId)
        {
            return await _dbContext.EventParticipants
                .Where(ep => ep.EventId == eventId)
                .CountAsync();
        }
    }
}
