using EventsWebApplication.Core.Interfaces;
using EventsWebApplication.Core.Models;
using EventsWebApplication.Infrasturture.Data;
using Microsoft.EntityFrameworkCore;

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
                .Include(e => e.Participants)
                .ToListAsync();
        }

        public async Task<Event> GetEventByIdAsync(int id)
        {
            return await _dbContext.Events
                .Include(e => e.Participants)
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<Event> GetEventByNameAsync(string title)
        {
            return await _dbContext.Events
                .Include(e => e.Participants)
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
    }
}
