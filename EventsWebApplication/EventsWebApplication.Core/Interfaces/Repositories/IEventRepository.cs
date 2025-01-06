using EventsWebApplication.Core.DTOs;
using EventsWebApplication.Core.Models;

namespace EventsWebApplication.Core.Interfaces.Repositories
{
    public interface IEventRepository : IRepository<Event>
    {
        Task<Event> GetEventByIdAsync(int id);
        Task<Event> GetEventByTitleAsync(string title);
        IQueryable<Event> GetEventsWithFilterQuery(EventFilterDto filter);
        Task<int> GetCurrentParticipantsAsync(int eventId);
    }
}
