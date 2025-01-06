using EventsWebApplication.Core.Models;

namespace EventsWebApplication.Core.Interfaces.Repositories
{
    public interface INotificationRepository : IRepository<Notification>
    {
        Task<IEnumerable<Notification>> GetNotificationsByParticipantIdAsync(int participantId);
        Task NotifyParticipantsAboutEventChange(IEnumerable<Participant> participants, string message);
    }
}
