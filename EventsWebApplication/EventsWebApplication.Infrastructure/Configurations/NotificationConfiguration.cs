using EventsWebApplication.Core.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventsWebApplication.Infrastructure.Data.Configurations
{
    public class NotificationConfiguration : IEntityTypeConfiguration<Notification>
    {
        public void Configure(EntityTypeBuilder<Notification> builder)
        {
            builder.HasOne(n => n.Participant)
                   .WithMany(p => p.Notifications)
                   .HasForeignKey(n => n.ParticipantId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
