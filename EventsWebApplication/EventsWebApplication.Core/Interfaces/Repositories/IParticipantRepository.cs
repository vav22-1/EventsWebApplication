using EventsWebApplication.Core.Models;

namespace EventsWebApplication.Core.Interfaces.Repositories
{
    public interface IParticipantRepository : IRepository<Participant>
    {
        Task<IEnumerable<Participant>> GetParticipantsByEventIdAsync(int eventId);
        Task<Participant> GetParticipantByIdAsync(int id);
        Task AddParticipantToEventAsync(int eventId, int participantId);
        Task RemoveParticipantFromEventAsync(int eventId, int participantId);
    }
}
