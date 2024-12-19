using EventsWebApplication.Core.DTOs;
using EventsWebApplication.Core.Interfaces;
using EventsWebApplication.Core.Models;
using EventsWebApplication.Infrastructure.Data;

namespace EventsWebApplication.Infrastructure.Repositories
{
    public class EventRepository : IEventRepository
    {
        private EventAppDbContext _dbContext;
        public EventRepository(EventAppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Event>> GetAllEventsAsync()
        {
            return await _dbContext.Events
                .Include(e => e.EventParticipants)
                .ThenInclude(ep => ep.Participant)
                .ToListAsync();
        }

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
        public async Task AddEventAsync(Event newEvent)
        {
            await _dbContext.Events.AddAsync(newEvent);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateEventAsync(Event newEvent)
        {
            _dbContext.Events.Update(newEvent);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteEventAsync(int id)
        {
            var eventToDelete = await _dbContext.Events.FindAsync(id);
            if (eventToDelete != null)
            {
                _dbContext.Events.Remove(eventToDelete);
                await _dbContext.SaveChangesAsync();
            }
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
        public async Task<int> GetAvailableSeatsAsync(int eventId)
        {
            var eventEntity = await _dbContext.Events.FindAsync(eventId);
            if (eventEntity != null)
            {
                var currentParticipants = await GetCurrentParticipantsAsync(eventId);
                return eventEntity.MaxParticipants - currentParticipants;
            }
            return 0;
        }
    }
}
