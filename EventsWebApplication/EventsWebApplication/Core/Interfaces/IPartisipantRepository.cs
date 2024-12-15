using EventsWebApplication.Core.Models;

namespace EventsWebApplication.Core.Interfaces
{
    public interface IPartisipantRepository
    {
        Task<IEnumerable<Participant>> GetParticipantsByEventIdAsync(int eventId);
        Task<Participant> GetParticipantByIdAsync(int id);
        Task AddParticipantAsync(Participant participant);
        Task DeleteParticipantAsync(int id);
        Task UpdateParticipantAsync(Participant participant);
        Task AddParticipantToEventAsync(int eventId, int participantId);
        Task RemoveParticipantFromEventAsync(int eventId, int participantId);
    }
}
