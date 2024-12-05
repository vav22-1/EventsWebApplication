using EventsWebApplication.Core.Interfaces;
using EventsWebApplication.Core.Models;
using EventsWebApplication.Infrasturture.Data;
using Microsoft.EntityFrameworkCore;

namespace EventsWebApplication.Infrastructure.Repositories
{
    public class ParticipantRepository : IPartisipantRepository
    {
        private EventAppDbContext _dbContext;

        public async Task<Participant> GetParticipantByIdAsync(int id)
        {
            return await _dbContext.Participants
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<IEnumerable<Participant>> GetParticipantsByEventIdAsync(int eventId)
        {
            return await _dbContext.Participants
                .ToListAsync();
        }
        public ParticipantRepository(EventAppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task AddParticipantAsync(Participant participant)
        {
            await _dbContext.Participants.AddAsync(participant);
        }

        public async Task DeleteParticipantAsync(int id)
        {
            var participant = await _dbContext.Participants.FindAsync(id);
            if (participant != null)
            {
                _dbContext.Participants.Remove(participant);
            }
        }
    }
}
