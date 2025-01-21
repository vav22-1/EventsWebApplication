using EventsWebApplication.Core.Models;

namespace EventsWebApplication.Core.Interfaces.Repositories
{
    public interface IEventRepository : IRepository<Event>
    {
        Task<Event> GetEventByIdAsync(int id);
        Task<Event> GetEventByTitleAsync(string title);
        Task<(IEnumerable<Event>, int totalCount)> GetPaginatedEventsAsync(EventFilter? filter, int page, int pageSize);

        Task<(IEnumerable<Event>, int totalCount)> GetEventsByParticipantIdWithFilterAsync(int participantId, EventFilter? filter, int page, int pageSize);
        Task<int> GetCurrentParticipantsAsync(int eventId);
    }
}
