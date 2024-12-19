using EventsWebApplication.Core.Interfaces;
using EventsWebApplication.Core.Models;
using EventsWebApplication.Infrastructure.Data;

namespace EventsWebApplication.Infrastructure.Repositories
{
    public class ParticipantRepository : IPartisipantRepository
    {
        private EventAppDbContext _dbContext;
        public ParticipantRepository(EventAppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

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
        public async Task AddParticipantAsync(Participant participant)
        {
            await _dbContext.Participants.AddAsync(participant);
        }
        public async Task UpdateParticipantAsync(Participant participant)
        {
            _dbContext.Participants.Update(participant);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteParticipantAsync(int id)
        {
            var participant = await _dbContext.Participants.FindAsync(id);
            if (participant != null)
            {
                _dbContext.Participants.Remove(participant);
            }
        }
        public async Task AddParticipantToEventAsync(int eventId, int participantId)
        {
            var eventParticipant = new EventParticipant
            {
                EventId = eventId,
                ParticipantId = participantId
            };

            await _dbContext.EventParticipants.AddAsync(eventParticipant);
            await _dbContext.SaveChangesAsync();
        }
        public async Task RemoveParticipantFromEventAsync(int eventId, int participantId)
        {
            var eventParticipant = await _dbContext.EventParticipants
                .FirstOrDefaultAsync(ep => ep.EventId == eventId && ep.ParticipantId == participantId);

            if (eventParticipant != null)
            {
                _dbContext.EventParticipants.Remove(eventParticipant);
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}
