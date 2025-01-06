using EventsWebApplication.Core.Models;

namespace EventsWebApplication.Infrastructure.Data;

public class EventAppDbContext : DbContext
{
    public EventAppDbContext(DbContextOptions<EventAppDbContext> options) : base(options) { }
    public DbSet<Event> Events { get; set; }
    public DbSet<Participant> Participants { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Notification> Notifications { get; set; }
    public DbSet<EventParticipant> EventParticipants { get; set; }

    //Конфигурация сущностей разнесена из OnModelCreating по отдельным файлам 

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(EventAppDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}
