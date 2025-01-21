using EventsWebApplication.Application.UseCases.EventUseCases;
using EventsWebApplication.Application.DTOs.EventDTOs;
using EventsWebApplication.Core.Models;

namespace EventsWebApplication.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EventsController : ControllerBase
    {
        private readonly GetAllEventsUseCase _getAllEventsUseCase;
        private readonly GetEventByIdUseCase _getEventByIdUseCase;
        private readonly GetEventByNameUseCase _getEventByNameUseCase;
        private readonly AddEventUseCase _addEventUseCase;
        private readonly UpdateEventUseCase _updateEventUseCase;
        private readonly DeleteEventUseCase _deleteEventUseCase;
        private readonly UploadEventImageUseCase _uploadEventImageUseCase;
        private readonly GetEventImageUseCase _getEventImageUseCase;
        private readonly GetPaginatedEventsUseCase _getPaginatedEventsUseCase;
        private readonly GetEventsByParticipantWithFilterUseCase _getEventsByParticipantWithFilterUseCase;
        private readonly GetAvailableSeatsUseCase _getAvailableSeatsUseCase;

        public EventsController(
            GetAllEventsUseCase getAllEventsUseCase,
            GetEventByIdUseCase getEventByIdUseCase,
            GetEventByNameUseCase getEventByNameUseCase,
            AddEventUseCase addEventUseCase,
            UpdateEventUseCase updateEventUseCase,
            DeleteEventUseCase deleteEventUseCase,
            UploadEventImageUseCase uploadEventImageUseCase,
            GetEventImageUseCase getEventImageUseCase,
            GetPaginatedEventsUseCase getPaginatedEventsUseCase,
            GetEventsByParticipantWithFilterUseCase getEventsByParticipantWithFilterUseCase,
            GetAvailableSeatsUseCase getAvailableSeatsUseCase)
        {
            _getAllEventsUseCase = getAllEventsUseCase;
            _getEventByIdUseCase = getEventByIdUseCase;
            _getEventByNameUseCase = getEventByNameUseCase;
            _addEventUseCase = addEventUseCase;
            _updateEventUseCase = updateEventUseCase;
            _deleteEventUseCase = deleteEventUseCase;
            _uploadEventImageUseCase = uploadEventImageUseCase;
            _getEventImageUseCase = getEventImageUseCase;
            _getPaginatedEventsUseCase = getPaginatedEventsUseCase;
            _getEventsByParticipantWithFilterUseCase = getEventsByParticipantWithFilterUseCase;
            _getAvailableSeatsUseCase = getAvailableSeatsUseCase;
        }

        [HttpGet]
        [Authorize(Policy = "UserPolicy")]
        public async Task<IActionResult> GetAllEvents()
        {
            var eventDtos = await _getAllEventsUseCase.ExecuteAsync(null);
            return Ok(eventDtos);
        }

        [HttpGet("{id}")]
        [Authorize(Policy = "UserPolicy")]
        public async Task<IActionResult> GetEventById(int id)
        {
            var eventDto = await _getEventByIdUseCase.ExecuteAsync(new GetEventByIdDto { Id = id });
            return Ok(eventDto);
        }

        [HttpGet("name/{name}")]
        [Authorize(Policy = "UserPolicy")]
        public async Task<IActionResult> GetEventByName(string name)
        {
            var eventDto = await _getEventByNameUseCase.ExecuteAsync(new GetEventByNameDto { Name = name });
            return Ok(eventDto);
        }

        [HttpPost]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> AddEvent([FromBody] EventRequestDto newEventDto)
        {
            var createdEventDto = await _addEventUseCase.ExecuteAsync(newEventDto);
            return CreatedAtAction(nameof(GetEventById), new { id = createdEventDto.Id }, createdEventDto);
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> UpdateEvent(int id, [FromBody] EventRequestDto updatedEventDto)
        {
            await _updateEventUseCase.ExecuteAsync(new EventUpdateRequestDto { Id = id, UpdatedEventDto = updatedEventDto });
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> DeleteEvent(int id)
        {
            await _deleteEventUseCase.ExecuteAsync(new DeleteEventDto { Id = id });
            return NoContent();
        }

        [HttpPost("{id}/upload-image")]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> UploadImageToEvent(int id, IFormFile image)
        {
            byte[] imageData = null;
            using (var memoryStream = new MemoryStream())
            {
                await image.CopyToAsync(memoryStream);
                imageData = memoryStream.ToArray();
            }

            var imagePath = await _uploadEventImageUseCase.ExecuteAsync(new UploadImageRequestDto { EventId = id, ImageData = imageData });
            return Ok(new { ImagePath = imagePath });
        }

        [HttpGet("{eventId}/image")]
        [Authorize(Policy = "UserPolicy")]
        public IActionResult GetEventImage(int eventId)
        {
            var imageData = _getEventImageUseCase.ExecuteAsync(new GetEventByIdDto { Id = eventId }).Result;
            return File(imageData, "image/jpeg");
        }

        [HttpGet("filter")]
        [Authorize(Policy = "UserPolicy")]
        public async Task<IActionResult> GetEventsByFilter([FromQuery] EventFilter filter, int page = 1, int pageSize = 8)
        {
            var paginatedResult = await _getPaginatedEventsUseCase.ExecuteAsync(new PaginatedEventRequestDto { Filter = filter, Page = page, PageSize = pageSize});
            return Ok(paginatedResult);
        }

        [HttpGet("{participantId}/events")]
        [Authorize(Policy = "UserPolicy")]
        public async Task<IActionResult> GetEventsByParticipantWithFilter(int participantId, [FromQuery] EventFilter filter, int page = 1, int pageSize = 8)
        {
            var paginatedResult = await _getEventsByParticipantWithFilterUseCase.ExecuteAsync(new GetParticipantEventsRequestDto { ParticipantId = participantId, FilterAndPagination = new PaginatedEventRequestDto { Filter = filter, Page = page, PageSize = pageSize } });
            return Ok(paginatedResult);
        }

        [HttpGet("{id}/available-seats")]
        [Authorize(Policy = "UserPolicy")]
        public async Task<IActionResult> GetAvailableSeats(int id)
        {
            var availableSeats = await _getAvailableSeatsUseCase.ExecuteAsync(new GetEventByIdDto { Id = id });
            return Ok(new { AvailableSeats = availableSeats });
        }
    }
}
