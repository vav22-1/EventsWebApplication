﻿using EventsWebApplication.Core.Models;
using EventsWebApplication.Infrastructure.Repositories;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace EventsWebApplication.Tests.Repositories
{
    public class EventRepositoryTests
    {
        [Fact]
        public async Task GetAllEventsAsync_ShouldReturnAllEvents()
        {
            var dbContext = TestDbContextHelper.GetInMemoryDbContext();
            var repository = new EventRepository(dbContext);

            var events = await repository.GetAllAsync();

            events.Should().HaveCount(2);
        }

        [Fact]
        public async Task AddEventAsync_ShouldAddNewEvent()
        {
            var dbContext = TestDbContextHelper.GetInMemoryDbContext();
            var repository = new EventRepository(dbContext);

            var newEvent = new Event
            {
                Title = "New Event",
                Category = "Tech",
                Location = "SF",
                DateAndTime = DateTime.Now.AddMonths(1),
                MaxParticipants = 200
            };

            await repository.AddAsync(newEvent);
            await dbContext.SaveChangesAsync();

            dbContext.Events.Should().HaveCount(3);
        }
    }
}
