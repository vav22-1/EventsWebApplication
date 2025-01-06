using EventsWebApplication.Core.Interfaces.Repositories;
using EventsWebApplication.Core.Models;
using EventsWebApplication.Infrastructure.Data;

namespace EventsWebApplication.Infrastructure.Repositories
{
    public class NotificationRepository : Repository<Notification>, INotificationRepository
    {
        public NotificationRepository(EventAppDbContext dbContext) : base(dbContext) { }

        public async Task<IEnumerable<Notification>> GetNotificationsByParticipantIdAsync(int participantId)
        {
            return await _dbContext.Notifications
                .Where(n => n.ParticipantId == participantId)
                .OrderByDescending(n => n.CreatedAt)
                .ToListAsync();
        }

        public async Task NotifyParticipantsAboutEventChange(IEnumerable<Participant> participants, string message)
        {

            foreach (var participant in participants)
            {
                var notification = new Notification
                {
                    ParticipantId = participant.Id,
                    Message = message,
                    CreatedAt = DateTime.Now
                };

                await _dbContext.Notifications.AddAsync(notification);
            }
        }

    }
}
