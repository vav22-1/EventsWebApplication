using EventsWebApplication.Core.DTOs;
using EventsWebApplication.Core.Models;
using EventsWebApplication.Infrastructure.Repositories;
using EventsWebApplication.Tests;
using FluentAssertions;
using Xunit;

namespace EventsWebApplication.Tests.UseCases
{
    public class EventUseCasesTests
    {
        [Fact]
        public async Task GetAllEvents_ShouldReturnAllEvents()
        {
            var dbContext = TestDbContextHelper.GetInMemoryDbContext();
            var repository = new EventRepository(dbContext);

            var events = await repository.GetAllEventsAsync();

            events.Should().NotBeNull();
            events.Should().HaveCount(2);
        }

        [Fact]
        public async Task GetEventById_ShouldReturnCorrectEvent()
        {
            var dbContext = TestDbContextHelper.GetInMemoryDbContext();
            var repository = new EventRepository(dbContext);

            var eventEntity = await repository.GetEventByIdAsync(1);

            eventEntity.Should().NotBeNull();
            eventEntity.Title.Should().Be("Event 1");
        }

        [Fact]
        public async Task GetEventByTitle_ShouldReturnCorrectEvent()
        {
            var dbContext = TestDbContextHelper.GetInMemoryDbContext();
            var repository = new EventRepository(dbContext);

            var eventEntity = await repository.GetEventByTitleAsync("Event 2");

            eventEntity.Should().NotBeNull();
            eventEntity.Category.Should().Be("Спорт");
        }

        [Fact]
        public async Task AddEvent_ShouldAddNewEvent()
        {
            var dbContext = TestDbContextHelper.GetInMemoryDbContext();
            var repository = new EventRepository(dbContext);

            var newEvent = new Event
            {
                Title = "New Event",
                Category = "Технологии",
                Location = "Полоцк",
                DateAndTime = DateTime.Now.AddMonths(1),
                MaxParticipants = 200
            };

            await repository.AddEventAsync(newEvent);

            dbContext.Events.Should().HaveCount(3);
        }

        [Fact]
        public async Task UpdateEvent_ShouldUpdateExistingEvent()
        {
            var dbContext = TestDbContextHelper.GetInMemoryDbContext();
            var repository = new EventRepository(dbContext);

            var initialEvent = new Event
            {
                Id = 3,
                Title = "Event 3",
                Category = "Технологии",
                Location = "Минск",
                DateAndTime = DateTime.Now,
                MaxParticipants = 100
            };
            await repository.AddEventAsync(initialEvent);

            var eventToUpdate = await repository.GetEventByIdAsync(3);
            eventToUpdate.Title = "Updated Event 3";
            eventToUpdate.Category = "Музыка";
            eventToUpdate.Location = "Витебск";
            eventToUpdate.DateAndTime = DateTime.Now.AddMonths(1);
            eventToUpdate.MaxParticipants = 120;

            await repository.UpdateEventAsync(eventToUpdate);

            var eventEntity = await repository.GetEventByIdAsync(3);
            eventEntity.Should().NotBeNull();
            eventEntity.Title.Should().Be("Updated Event 3");
            eventEntity.Category.Should().Be("Музыка");
            eventEntity.Location.Should().Be("Витебск");
            eventEntity.MaxParticipants.Should().Be(120);
        }

        [Fact]
        public async Task DeleteEvent_ShouldDeleteEvent()
        {
            var dbContext = TestDbContextHelper.GetInMemoryDbContext();
            var repository = new EventRepository(dbContext);

            await repository.DeleteEventAsync(1);

            var deletedEvent = await dbContext.Events.FindAsync(1);
            deletedEvent.Should().BeNull();
        }

        [Fact]
        public void GetEventsWithFilterQuery_ShouldFilterEventsByCategory()
        {
            var dbContext = TestDbContextHelper.GetInMemoryDbContext();
            var repository = new EventRepository(dbContext);

            var filter = new EventFilterDto { FilterCategory = "Музыка" };

            var filteredEvents = repository.GetEventsWithFilterQuery(filter).ToList();

            filteredEvents.Should().HaveCount(1);
            filteredEvents[0].Category.Should().Be("Музыка");
        }

        [Fact]
        public async Task GetCurrentParticipants_ShouldReturnCurrentParticipants()
        {
            var dbContext = TestDbContextHelper.GetInMemoryDbContext();
            var repository = new EventRepository(dbContext);

            var testParticipant = new Participant
            {
                Id = 1,
                FirstName = "Никита",
                LastName = "Юркевич",
                Email = "nik.yurkevich@example.com",
                DateOfRegistration = DateTime.Now
            };
            dbContext.Participants.Add(testParticipant);

            dbContext.EventParticipants.Add(new EventParticipant { EventId = 1, ParticipantId = 1 });
            await dbContext.SaveChangesAsync();

            var participantsCount = await repository.GetCurrentParticipantsAsync(1);

            participantsCount.Should().Be(1);
        }

        [Fact]
        public async Task GetAvailableSeats_ShouldReturnCorrectAvailableSeats()
        {
            var dbContext = TestDbContextHelper.GetInMemoryDbContext();
            var repository = new EventRepository(dbContext);

            var testParticipant = new Participant
            {
                Id = 1,
                FirstName = "Никита",
                LastName = "Юркевич",
                Email = "nik.yurkevich@example.com",
                DateOfRegistration = DateTime.Now
            };
            dbContext.Participants.Add(testParticipant);

            dbContext.EventParticipants.Add(new EventParticipant { EventId = 1, ParticipantId = 1 });

            await dbContext.SaveChangesAsync();

            var availableSeats = await repository.GetAvailableSeatsAsync(1);

            availableSeats.Should().Be(99);
        }
    }
}
