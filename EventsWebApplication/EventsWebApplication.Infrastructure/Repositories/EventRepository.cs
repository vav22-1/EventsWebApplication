using EventsWebApplication.Core.DTOs.EventDTOs;
using EventsWebApplication.Core.Interfaces.Repositories;
using EventsWebApplication.Core.Models;
using EventsWebApplication.Infrastructure.Data;
using System.Linq.Expressions;

namespace EventsWebApplication.Infrastructure.Repositories
{
    public class EventRepository : Repository<Event>, IEventRepository
    {
        public EventRepository(EventAppDbContext dbContext) : base(dbContext) { }

        private static Expression<Func<Event, bool>> BuildEventFilter(EventFilterDto? filter)
        {
            return e =>
                (string.IsNullOrWhiteSpace(filter.Title) || e.Title.StartsWith(filter.Title)) &&
                (!filter.FilterDate.HasValue || e.DateAndTime.Date == filter.FilterDate.Value.Date) &&
                (string.IsNullOrWhiteSpace(filter.FilterLocation) || e.Location.StartsWith(filter.FilterLocation)) &&
                (string.IsNullOrWhiteSpace(filter.FilterCategory) || e.Category.StartsWith(filter.FilterCategory));
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

        public async Task<(IEnumerable<Event>, int totalCount)> GetPaginatedEventsAsync(EventFilterDto? filter, int page, int pageSize)
        {
            var filterExpression = BuildEventFilter(filter);
            var query = _dbContext.Events.Where(filterExpression);

            var totalCount = await query.CountAsync();
            var items = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, totalCount);
        }

        public async Task<(IEnumerable<Event>, int totalCount)> GetEventsByParticipantIdWithFilterAsync(int participantId, EventFilterDto? filter, int page, int pageSize)
        {
            var filterExpression = BuildEventFilter(filter);
            var query = _dbContext.EventParticipants
                .Where(ep => ep.ParticipantId == participantId)
                .Select(ep => ep.Event)
                .Where(filterExpression);

            var totalCount = await query.CountAsync();
            var events = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (events, totalCount);
        }

        public IQueryable<Event> GetEventsByIds(IEnumerable<int> eventIds)
        {
            return _dbContext.Events.Where(e => eventIds.Contains(e.Id));
        }

        public async Task<int> GetCurrentParticipantsAsync(int eventId)
        {
            return await _dbContext.EventParticipants
                .Where(ep => ep.EventId == eventId)
                .CountAsync();
        }
    }
}
