using EventsWebApplication.Core.DTOs;
using EventsWebApplication.Core.Interfaces;
using EventsWebApplication.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace EventsWebApplication.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EventsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public EventsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllEvents()
        {
            var events = await _unitOfWork.Events.GetAllEventsAsync();
            return Ok(events);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetEventById(int id)
        {
            var eventItem = await _unitOfWork.Events.GetEventByIdAsync(id);
            if (eventItem == null) return NotFound();
            return Ok(eventItem);
        }

        [HttpPost]
        public async Task<IActionResult> AddEvent([FromBody] EventDto newEventDto)
        {
            var newEvent = new Event
            {
                Title = newEventDto.Title,
                Description = newEventDto.Description,
                DateAndTime = newEventDto.DateAndTime,
                Location = newEventDto.Location,
                Category = newEventDto.Category,
                MaxParticipants = newEventDto.MaxParticipants,
                ImagePath = newEventDto.ImagePath,
            };
            if (!ModelState.IsValid) return BadRequest(ModelState);

            await _unitOfWork.Events.AddEventAsync(newEvent);
            await _unitOfWork.CompleteAsync();
            return CreatedAtAction(nameof(GetEventById), new { id = newEvent.Id }, newEvent);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEvent(int id, [FromBody] EventDto updatedEventDto)
        {
            var eventToUpdate = await _unitOfWork.Events.GetEventByIdAsync(id);
            if (eventToUpdate == null) return NotFound();

            eventToUpdate.Title = updatedEventDto.Title;
            eventToUpdate.Description = updatedEventDto.Description;
            eventToUpdate.DateAndTime = updatedEventDto.DateAndTime;
            eventToUpdate.Location = updatedEventDto.Location;
            eventToUpdate.Category = updatedEventDto.Category;
            eventToUpdate.MaxParticipants = updatedEventDto.MaxParticipants;
            eventToUpdate.ImagePath = updatedEventDto.ImagePath;

            await _unitOfWork.Events.UpdateEventAsync(eventToUpdate);
            await _unitOfWork.CompleteAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEvent(int id)
        {
            await _unitOfWork.Events.DeleteEventAsync(id);
            await _unitOfWork.CompleteAsync();
            return NoContent();
        }
    }
}
