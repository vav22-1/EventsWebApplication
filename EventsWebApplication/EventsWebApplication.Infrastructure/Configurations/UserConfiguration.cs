using EventsWebApplication.Core.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventsWebApplication.Infrastructure.Data.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasOne(u => u.Participant)
                   .WithOne(p => p.User)
                   .HasForeignKey<User>(u => u.ParticipantId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

