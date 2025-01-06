using EventsWebApplication.Core.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventsWebApplication.Infrastructure.Data.Configurations
{
    public class EventParticipantConfiguration : IEntityTypeConfiguration<EventParticipant>
    {
        public void Configure(EntityTypeBuilder<EventParticipant> builder)
        {
            builder.HasKey(ep => new { ep.EventId, ep.ParticipantId });

            builder.HasOne(ep => ep.Event)
                   .WithMany(e => e.EventParticipants)
                   .HasForeignKey(ep => ep.EventId);

            builder.HasOne(ep => ep.Participant)
                   .WithMany(p => p.EventParticipants)
                   .HasForeignKey(ep => ep.ParticipantId);
        }
    }
}
