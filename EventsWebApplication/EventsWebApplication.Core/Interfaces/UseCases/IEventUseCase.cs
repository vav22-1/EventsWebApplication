using EventsWebApplication.Core.DTOs;
using Microsoft.AspNetCore.Http;

namespace EventsWebApplication.Core.Interfaces.UseCases
{
    public interface IEventUseCase
    {
        Task<IEnumerable<EventResponseDto>> GetAllEventsAsync();
        Task<EventResponseDto> GetEventByIdAsync(int id);
        Task<EventResponseDto> GetEventByNameAsync(string name);
        Task<EventResponseDto> AddEventAsync(EventRequestDto newEventDto);
        Task UpdateEventAsync(int id, EventRequestDto updatedEventDto);
        Task DeleteEventAsync(int id);
        Task<string> UploadEventImageAsync(int id, IFormFile image);
        FileContentResult GetEventImage(int eventId);
        Task<object> GetEventsByFilterAsync(EventFilterDto filter, int page, int pageSize);
        Task<int> GetAvailableSeatsAsync(int id);
    }
}
