using AutoMapper;
using EventsWebApplication.Core.DTOs;
using EventsWebApplication.Core.Interfaces;
using EventsWebApplication.Core.Models;
using EventsWebApplication.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EventsWebApplication.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EventsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ImageSaver _imageSaver;
        private readonly IMapper _mapper;

        public EventsController(IUnitOfWork unitOfWork, ImageSaver imageSaver, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _imageSaver = imageSaver;
            _mapper = mapper;
        }

        
        [HttpGet]
        [Authorize(Policy = "UserPolicy")]
        public async Task<IActionResult> GetAllEvents()
        {
            var events = await _unitOfWork.Events.GetAllEventsAsync();
            var eventDtos = _mapper.Map<IEnumerable<EventResponseDto>>(events);
            return Ok(eventDtos);
        }

        [HttpGet("{id}")]
        [Authorize(Policy = "UserPolicy")]
        public async Task<IActionResult> GetEventById(int id)
        {
            var eventItem = await _unitOfWork.Events.GetEventByIdAsync(id);
            if (eventItem == null) return NotFound();
            var eventDto = _mapper.Map<EventResponseDto>(eventItem);
            return Ok(eventDto);
        }

        [HttpGet("name/{name}")]
        [Authorize(Policy = "UserPolicy")]
        public async Task<IActionResult> GetEventByName(string name)
        {
            var eventItem = await _unitOfWork.Events.GetEventByTitleAsync(name);
            if (eventItem == null) return NotFound();
            var eventDto = _mapper.Map<EventResponseDto>(eventItem);
            return Ok(eventDto);
        }


        [HttpPost]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> AddEvent([FromBody] EventRequestDto newEventDto)
        {
            var newEvent = _mapper.Map<Event>(newEventDto);

            await _unitOfWork.Events.AddEventAsync(newEvent);
            await _unitOfWork.CompleteAsync();

            var createdEventDto = _mapper.Map<EventResponseDto>(newEvent);
            return CreatedAtAction(nameof(GetEventById), new { id = newEvent.Id }, createdEventDto);
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> UpdateEvent(int id, [FromBody] EventRequestDto updatedEventDto)
        {
            Event updatedEvent = await _unitOfWork.Events.GetEventByIdAsync(id);
            var eventToUpdate = await _unitOfWork.Events.GetEventByIdAsync(id);
            if (eventToUpdate == null) return NotFound();

            _mapper.Map(updatedEventDto, eventToUpdate);
            await _unitOfWork.Notifications.NotifyParticipantsAboutEventChange(await _unitOfWork.Participants.GetParticipantsByEventIdAsync(id), $"Событие {updatedEvent.Title} было изменено");
            await _unitOfWork.Events.UpdateEventAsync(eventToUpdate);
            await _unitOfWork.CompleteAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> DeleteEvent(int id)
        {
            Event deletedEvent = await _unitOfWork.Events.GetEventByIdAsync(id);
            await _unitOfWork.Notifications.NotifyParticipantsAboutEventChange(await _unitOfWork.Participants.GetParticipantsByEventIdAsync(id), $"Событие {deletedEvent.Title} было отменено");
            await _unitOfWork.Events.DeleteEventAsync(id);
            await _unitOfWork.CompleteAsync();
            return NoContent();
        }

        [HttpPost("{id}/upload-image")]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> UploadImageToEvent(int id, IFormFile image)
        {
            var eventToUpdate = await _unitOfWork.Events.GetEventByIdAsync(id);
            if (eventToUpdate == null) return NotFound();
            var imagePath = await _imageSaver.SaveImageAsync(image);
            if(string.IsNullOrEmpty(imagePath))
            {
                return BadRequest("Ошибка загрузки изображения");
            }
            eventToUpdate.ImagePath = imagePath;
            await _unitOfWork.CompleteAsync();

            return Ok(new { ImagePath = imagePath});
        }

        [HttpGet("{eventId}/image")]
        [Authorize(Policy = "UserPolicy")]
        public IActionResult GetEventImage(int eventId)
        {
            var eventItem = _unitOfWork.Events.GetEventByIdAsync(eventId).Result;
            if (eventItem == null || string.IsNullOrEmpty(eventItem.ImagePath))
            {
                return NotFound("Изображение не найдено для этого события");
            }

            var imagePath = _imageSaver.GetImagePath(eventItem.ImagePath);
            if (!System.IO.File.Exists(imagePath))
            {
                return NotFound("Файл изображения этого сервера не найден");
            }

            var image = System.IO.File.ReadAllBytes(imagePath);
            var contentType = "image/" + Path.GetExtension(imagePath).TrimStart('.');
            return File(image, contentType);
        }

        [HttpGet("filter")]
        [Authorize(Policy = "UserPolicy")]
        public async Task<IActionResult> GetEventsByFilter([FromQuery] EventFilterDto filter, int page = 1, int pageSize = 10)
        {
            var query = _unitOfWork.Events.GetEventsWithFilterQuery(filter);

            var totalCount = await query.CountAsync();
            var items = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return Ok(new
            {
                items,
                totalPages = (int)Math.Ceiling(totalCount / (double)pageSize)
            });
        }


        [HttpGet("{id}/available-seats")]
        [Authorize(Policy = "UserPolicy")]
        public async Task<IActionResult> GetAvailableSeats(int id) 
        {
            var availableSeats = await _unitOfWork.Events.GetAvailableSeatsAsync(id);
            return Ok(new { AvailableSeats = availableSeats }); 
        }
    }
}
