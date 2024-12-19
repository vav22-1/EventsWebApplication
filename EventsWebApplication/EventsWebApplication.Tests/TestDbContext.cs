using Microsoft.EntityFrameworkCore;

using EventsWebApplication.Infrastructure.Data;
using EventsWebApplication.Core.Models;

namespace EventsWebApplication.Tests
{
    public static class TestDbContextHelper
    {
        public static EventAppDbContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<EventAppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var context = new EventAppDbContext(options);
            context.Database.EnsureCreated();

            SeedData(context);

            return context;
        }

        private static void SeedData(EventAppDbContext context)
        {
            context.Events.AddRange(new List<Event>
        {
            new Event { Id = 1, Title = "Event 1", Category = "Музыка", Location = "Витебск", DateAndTime = DateTime.Now, MaxParticipants = 100 },
            new Event { Id = 2, Title = "Event 2", Category = "Спорт", Location = "Минск", DateAndTime = DateTime.Now.AddDays(1), MaxParticipants = 50 },
        });
            context.SaveChanges();
        }
    }

}
