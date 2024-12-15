using EventsWebApplication.Core.Models;

namespace EventsWebApplication.Core.Interfaces
{
    public interface INotificationRepository
    {
        Task<IEnumerable<Notification>> GetNotificationsByParticipantIdAsync(int participantId);
        Task AddNotificationAsync(Notification notification);
        Task NotifyParticipantsAboutEventChange(IEnumerable<Participant> participants, string message);
    }
}
