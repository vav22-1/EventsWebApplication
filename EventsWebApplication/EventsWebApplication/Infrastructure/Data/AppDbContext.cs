using Microsoft.EntityFrameworkCore;
using EventsWebApplication.Core.Models;

namespace EventsWebApplication.Infrasturture.Data;

public class EventAppDbContext : DbContext
{
    public EventAppDbContext(DbContextOptions<EventAppDbContext> options) : base(options) { }
    public DbSet<Event> Events { get; set; }
    public DbSet<Participant> Participants { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Notification> Notifications { get; set; }
    public DbSet<EventParticipant> EventParticipants { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .HasOne(u => u.Participant)
            .WithOne(p => p.User)
            .HasForeignKey<User>(u => u.ParticipantId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<EventParticipant>()
                .HasKey(ep => new { ep.EventId, ep.ParticipantId });

        modelBuilder.Entity<EventParticipant>()
            .HasOne(ep => ep.Event)
            .WithMany(e => e.EventParticipants)
            .HasForeignKey(ep => ep.EventId);

        modelBuilder.Entity<EventParticipant>()
            .HasOne(ep => ep.Participant)
            .WithMany(p => p.EventParticipants)
            .HasForeignKey(ep => ep.ParticipantId);

        modelBuilder.Entity<Notification>()
        .HasOne(n => n.Participant)
        .WithMany(p => p.Notifications)
        .HasForeignKey(n => n.ParticipantId)
        .OnDelete(DeleteBehavior.Cascade);

        base.OnModelCreating(modelBuilder);
    }
}
