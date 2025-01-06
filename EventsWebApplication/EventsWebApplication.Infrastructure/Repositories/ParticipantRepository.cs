using EventsWebApplication.Core.Interfaces.Repositories;
using EventsWebApplication.Core.Models;
using EventsWebApplication.Infrastructure.Data;

namespace EventsWebApplication.Infrastructure.Repositories
{
    public class ParticipantRepository : Repository<Participant>, IPartisipantRepository
    {
        public ParticipantRepository(EventAppDbContext dbContext) : base(dbContext) { }


        public async Task<Participant> GetParticipantByIdAsync(int id)
        {
            return await _dbContext.Participants
                .Include(ep => ep.EventParticipants)
                .ThenInclude(ep => ep.Event)
                .FirstOrDefaultAsync(ep => ep.Id == id);
        }

        public async Task<IEnumerable<Participant>> GetParticipantsByEventIdAsync(int eventId)
        {
            return await _dbContext.EventParticipants
                .Where(ep => ep.EventId == eventId)
                .Include(ep => ep.Participant)
                .Select(ep => ep.Participant)
                .ToListAsync();
        }

        public async Task AddParticipantToEventAsync(int eventId, int participantId)
        {
            var eventParticipant = new EventParticipant
            {
                EventId = eventId,
                ParticipantId = participantId
            };

            await _dbContext.EventParticipants.AddAsync(eventParticipant);
        }
        public async Task RemoveParticipantFromEventAsync(int eventId, int participantId)
        {
            var eventParticipant = await _dbContext.EventParticipants
                .FirstOrDefaultAsync(ep => ep.EventId == eventId && ep.ParticipantId == participantId);

            if (eventParticipant != null)
            {
                _dbContext.EventParticipants.Remove(eventParticipant);
            }
        }
    }
}
