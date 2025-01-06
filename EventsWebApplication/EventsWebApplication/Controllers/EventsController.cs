using EventsWebApplication.Core.DTOs;
using EventsWebApplication.Core.Interfaces.UseCases;

namespace EventsWebApplication.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EventsController : ControllerBase
    {
        private readonly IEventUseCase _eventUseCase;

        public EventsController(IEventUseCase eventUseCase)
        {
            _eventUseCase = eventUseCase;
        }

        [HttpGet]
        [Authorize(Policy = "UserPolicy")]
        public async Task<IActionResult> GetAllEvents()
        {
            var eventDtos = await _eventUseCase.GetAllEventsAsync();
            return Ok(eventDtos);
        }

        [HttpGet("{id}")]
        [Authorize(Policy = "UserPolicy")]
        public async Task<IActionResult> GetEventById(int id)
        {
            var eventDto = await _eventUseCase.GetEventByIdAsync(id);
            return Ok(eventDto);
        }

        [HttpGet("name/{name}")]
        [Authorize(Policy = "UserPolicy")]
        public async Task<IActionResult> GetEventByName(string name)
        {
            var eventDto = await _eventUseCase.GetEventByNameAsync(name);
            return Ok(eventDto);
        }

        [HttpPost]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> AddEvent([FromBody] EventRequestDto newEventDto)
        {
            var createdEventDto = await _eventUseCase.AddEventAsync(newEventDto);
            return CreatedAtAction(nameof(GetEventById), new { id = createdEventDto.Id }, createdEventDto);
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> UpdateEvent(int id, [FromBody] EventRequestDto updatedEventDto)
        {
            await _eventUseCase.UpdateEventAsync(id, updatedEventDto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> DeleteEvent(int id)
        {
            await _eventUseCase.DeleteEventAsync(id);
            return NoContent();
        }

        [HttpPost("{id}/upload-image")]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> UploadImageToEvent(int id, IFormFile image)
        {
            var imagePath = await _eventUseCase.UploadEventImageAsync(id, image);
            return Ok(new { ImagePath = imagePath });
        }

        [HttpGet("{eventId}/image")]
        [Authorize(Policy = "UserPolicy")]
        public IActionResult GetEventImage(int eventId)
        {
            var fileContentResult = _eventUseCase.GetEventImage(eventId);
            return fileContentResult;
        }

        [HttpGet("filter")]
        [Authorize(Policy = "UserPolicy")]
        public async Task<IActionResult> GetEventsByFilter([FromQuery] EventFilterDto filter, int page = 1, int pageSize = 8)
        {
            var paginatedResult = await _eventUseCase.GetEventsByFilterAsync(filter, page, pageSize);
            return Ok(paginatedResult);
        }

        [HttpGet("{id}/available-seats")]
        [Authorize(Policy = "UserPolicy")]
        public async Task<IActionResult> GetAvailableSeats(int id)
        {
            var availableSeats = await _eventUseCase.GetAvailableSeatsAsync(id);
            return Ok(new { AvailableSeats = availableSeats });
        }
    }
}
