using EventsWebApplication.Core.Models;
namespace EventsWebApplication.Core.Interfaces
{
    public interface IEventRepository
    {
        Task<IEnumerable<Event>> GetAllEventsAsync();
        Task<Event> GetEventByIdAsync(int id);
        Task<Event> GetEventByNameAsync(string name);
        Task AddEventAsync(Event newEvent);
        Task UpdateEventAsync(Event newEvent);
        Task DeleteEventAsync(int id);
    }
}
