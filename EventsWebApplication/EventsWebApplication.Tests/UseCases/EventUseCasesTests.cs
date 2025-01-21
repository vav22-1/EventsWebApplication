using EventsWebApplication.Application.DTOs.EventDTOs;
using EventsWebApplication.Application.UseCases.EventUseCases;
using FluentAssertions;
using Moq;
using AutoMapper;
using EventsWebApplication.Core.Interfaces;
using EventsWebApplication.Core.Models;

namespace EventsWebApplication.Tests.UseCases
{
    public class EventUseCasesTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IMapper> _mockMapper;

        public EventUseCasesTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockMapper = new Mock<IMapper>();
        }

        [Fact]
        public async Task GetAllEvents_ShouldReturnAllEvents()
        {
            // Arrange
            var useCase = new GetAllEventsUseCase(_mockUnitOfWork.Object, _mockMapper.Object);

            var eventEntities = new List<Event>
            {
                new Event { Id = 1, Title = "Event 1" },
                new Event { Id = 2, Title = "Event 2" }
            };
            _mockUnitOfWork.Setup(uow => uow.Events.GetAllAsync()).ReturnsAsync(eventEntities);

            var eventResponseDtos = new List<EventResponseDto>
            {
                new EventResponseDto { Id = "1", Title = "Event 1" },
                new EventResponseDto { Id = "2", Title = "Event 2" }
            };
            _mockMapper.Setup(mapper => mapper.Map<IEnumerable<EventResponseDto>>(eventEntities)).Returns(eventResponseDtos);

            // Act
            var result = await useCase.ExecuteAsync(null);

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(2);
            result.First().Title.Should().Be("Event 1");
        }

        [Fact]
        public async Task GetEventById_ShouldReturnCorrectEvent()
        {
            // Arrange
            var useCase = new GetEventByIdUseCase(_mockUnitOfWork.Object, _mockMapper.Object);
            var eventEntity = new Event { Id = 1, Title = "Event 1" };
            _mockUnitOfWork.Setup(uow => uow.Events.GetEventByIdAsync(1)).ReturnsAsync(eventEntity);
            var eventResponseDto = new EventResponseDto { Id = "1", Title = "Event 1" };
            _mockMapper.Setup(mapper => mapper.Map<EventResponseDto>(eventEntity)).Returns(eventResponseDto);

            var dto = new GetEventByIdDto { Id = 1 };

            // Act
            var result = await useCase.ExecuteAsync(dto);

            // Assert
            result.Should().NotBeNull();
            result.Title.Should().Be("Event 1");
        }

        [Fact]
        public async Task AddEvent_ShouldAddNewEvent()
        {
            // Arrange
            var useCase = new AddEventUseCase(_mockUnitOfWork.Object, _mockMapper.Object);
            var eventRequestDto = new EventRequestDto
            {
                Title = "New Event",
                Category = "Технологии",
                Location = "Полоцк",
                DateAndTime = DateTime.Now.AddMonths(1),
                MaxParticipants = 200
            };

            var newEvent = new Event
            {
                Title = "New Event",
                Category = "Технологии",
                Location = "Полоцк",
                DateAndTime = DateTime.Now.AddMonths(1),
                MaxParticipants = 200
            };

            var eventResponseDto = new EventResponseDto
            {
                Id = "1",
                Title = "New Event",
                Category = "Технологии",
                Location = "Полоцк",
                DateAndTime = DateTime.Now.AddMonths(1),
                MaxParticipants = 200
            };

            _mockMapper.Setup(mapper => mapper.Map<Event>(eventRequestDto)).Returns(newEvent);
            _mockMapper.Setup(mapper => mapper.Map<EventResponseDto>(newEvent)).Returns(eventResponseDto);
            _mockUnitOfWork.Setup(uow => uow.Events.AddAsync(newEvent)).Returns(Task.CompletedTask);

            // Act
            var result = await useCase.ExecuteAsync(eventRequestDto);

            // Assert
            result.Should().NotBeNull();
            result.Title.Should().Be("New Event");
            result.Category.Should().Be("Технологии");
            result.Location.Should().Be("Полоцк");

            _mockUnitOfWork.Verify(uow => uow.Events.AddAsync(newEvent), Times.Once);
        }


        [Fact]
        public async Task UpdateEvent_ShouldUpdateExistingEvent()
        {
            // Arrange
            var useCase = new UpdateEventUseCase(_mockUnitOfWork.Object, _mockMapper.Object);
            var eventToUpdate = new Event
            {
                Id = 3,
                Title = "Event 3",
                Category = "Технологии",
                Location = "Минск",
                DateAndTime = DateTime.Now,
                MaxParticipants = 100
            };

            var updateRequest = new EventUpdateRequestDto
            {
                Id = 3,
                UpdatedEventDto = new EventRequestDto
                {
                    Title = "Updated Event 3",
                    Category = "Музыка",
                    Location = "Витебск",
                    DateAndTime = DateTime.Now.AddMonths(1),
                    MaxParticipants = 120
                },
            };
            var participants = new List<Participant>
            {
                 new Participant { Id = 1, FirstName = "John", LastName = "Doe", Email = "john.doe@example.com" }
            };

            _mockUnitOfWork.Setup(uow => uow.Events.GetEventByIdAsync(3)).ReturnsAsync(eventToUpdate);
            _mockMapper.Setup(mapper => mapper.Map(updateRequest.UpdatedEventDto, eventToUpdate)).Returns(eventToUpdate);
            _mockUnitOfWork.Setup(uow => uow.Events.Update(eventToUpdate));

            _mockUnitOfWork.Setup(uow => uow.Participants.GetParticipantsByEventIdAsync(3)).ReturnsAsync(participants);
            _mockUnitOfWork.Setup(uow => uow.Notifications.NotifyParticipantsAboutEventChange(participants, It.IsAny<string>())).Returns(Task.CompletedTask);

            // Act
            var result = await useCase.ExecuteAsync(updateRequest);

            // Assert
            result.Should().Be("Событие обновлено успешно.");

            _mockUnitOfWork.Verify(uow => uow.Notifications.NotifyParticipantsAboutEventChange(participants, It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task DeleteEvent_ShouldDeleteEvent()
        {
            // Arrange
            var useCase = new DeleteEventUseCase(_mockUnitOfWork.Object);
            var deleteDto = new DeleteEventDto { Id = 1 };
            var eventToDelete = new Event { Id = 1, Title = "Event 1" };

            _mockUnitOfWork.Setup(uow => uow.Events.GetEventByIdAsync(1)).ReturnsAsync(eventToDelete);
            _mockUnitOfWork.Setup(uow => uow.Events.Delete(eventToDelete));

            // Act
            var result = await useCase.ExecuteAsync(deleteDto);

            // Assert
            result.Should().Be("Событие удалено.");
        }

        [Fact]
        public async Task GetAvailableSeats_ShouldReturnCorrectAvailableSeats()
        {
            // Arrange
            var useCase = new GetAvailableSeatsUseCase(_mockUnitOfWork.Object);
            var eventEntity = new Event { Id = 1, MaxParticipants = 100 };
            _mockUnitOfWork.Setup(uow => uow.Events.GetEventByIdAsync(1)).ReturnsAsync(eventEntity);
            _mockUnitOfWork.Setup(uow => uow.Events.GetCurrentParticipantsAsync(1)).ReturnsAsync(50);

            var dto = new GetEventByIdDto { Id = 1 };

            // Act
            var result = await useCase.ExecuteAsync(dto);

            // Assert
            result.Should().Be(50);
        }

        [Fact]
        public void GetEventsWithFilterQuery_ShouldFilterEventsByCategory()
        {
            // Arrange
            var useCase = new GetPaginatedEventsUseCase(_mockUnitOfWork.Object, _mockMapper.Object);
            var eventFilter = new EventFilter { FilterCategory = "Музыка" };
            var filter = new PaginatedEventRequestDto { Filter = eventFilter, Page = 1, PageSize = 8 };

            var events = new List<Event>
            {
                new Event { Category = "Музыка", Title = "Music Event 1" },
                new Event { Category = "Технологии", Title = "Tech Event 1" }
            };

            var eventResponseDtos = new List<EventResponseDto>
            {
                new EventResponseDto { Category = "Музыка", Title = "Music Event 1" }
            };

            _mockUnitOfWork.Setup(uow => uow.Events.GetPaginatedEventsAsync(eventFilter, 1, 8)).ReturnsAsync((events, 1));
            _mockMapper.Setup(mapper => mapper.Map<IEnumerable<EventResponseDto>>(events)).Returns(eventResponseDtos);

            // Act
            var result = useCase.ExecuteAsync(filter).Result;

            // Assert
            result.Items.Should().HaveCount(1);
            result.Items.First().Category.Should().Be("Музыка");
        }
    }
}
