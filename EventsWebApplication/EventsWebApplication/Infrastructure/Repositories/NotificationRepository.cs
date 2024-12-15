using EventsWebApplication.Core.Interfaces;
using EventsWebApplication.Core.Models;
using EventsWebApplication.Infrasturture.Data;
using Microsoft.EntityFrameworkCore;

namespace EventsWebApplication.Infrastructure.Repositories
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly EventAppDbContext _dbContext;
        public NotificationRepository(EventAppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<IEnumerable<Notification>> GetNotificationsByParticipantIdAsync(int participantId)
        {
            return await _dbContext.Notifications
                .Where(n => n.ParticipantId == participantId)
                .OrderByDescending(n => n.CreatedAt)
                .ToListAsync();
        }

        public async Task AddNotificationAsync(Notification notification)
        {
            await _dbContext.Notifications.AddAsync(notification);
            await _dbContext.SaveChangesAsync();
        }
        public async Task NotifyParticipantsAboutEventChange(IEnumerable<Participant> participants, string message)
        {

            foreach (var participant in participants)
            {
                var notification = new Notification
                {
                    ParticipantId = participant.Id,
                    Message = message,
                    CreatedAt = DateTime.UtcNow
                };

                await AddNotificationAsync(notification);
            }
        }

    }
}
