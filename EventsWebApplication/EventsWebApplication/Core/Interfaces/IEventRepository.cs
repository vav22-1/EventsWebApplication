using EventsWebApplication.Core.DTOs;
using EventsWebApplication.Core.Models;

namespace EventsWebApplication.Core.Interfaces
{
    public interface IEventRepository
    {
        Task<IEnumerable<Event>> GetAllEventsAsync();
        Task<Event> GetEventByIdAsync(int id);
        Task<Event> GetEventByTitleAsync(string title);
        Task AddEventAsync(Event newEvent);
        Task UpdateEventAsync(Event newEvent);
        Task DeleteEventAsync(int id);
        IQueryable<Event> GetEventsWithFilterQuery(EventFilterDto filter);
        Task<int> GetCurrentParticipantsAsync(int eventId);
        Task<int> GetAvailableSeatsAsync(int eventId);
    }
}
