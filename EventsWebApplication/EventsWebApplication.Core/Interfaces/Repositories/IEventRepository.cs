using EventsWebApplication.Core.DTOs.EventDTOs;
using EventsWebApplication.Core.Models;

namespace EventsWebApplication.Core.Interfaces.Repositories
{
    public interface IEventRepository : IRepository<Event>
    {
        Task<Event> GetEventByIdAsync(int id);
        Task<Event> GetEventByTitleAsync(string title);
        Task<(IEnumerable<Event>, int totalCount)> GetPaginatedEventsAsync(EventFilterDto? filter, int page, int pageSize);

        Task<(IEnumerable<Event>, int totalCount)> GetEventsByParticipantIdWithFilterAsync(int participantId, EventFilterDto? filter, int page, int pageSize);
        Task<int> GetCurrentParticipantsAsync(int eventId);
    }
}
