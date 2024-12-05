using Microsoft.EntityFrameworkCore;
using EventsWebApplication.Core.Models;

namespace EventsWebApplication.Infrasturture.Data;

public class EventAppDbContext : DbContext
{
    public EventAppDbContext(DbContextOptions<EventAppDbContext> options) : base(options) { }
    public DbSet<Event> Events { get; set; }
    public DbSet<Participant> Participants { get; set; }
}
