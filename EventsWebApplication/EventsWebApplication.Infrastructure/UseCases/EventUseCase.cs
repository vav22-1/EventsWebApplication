//Добавлены отдельные use cases для каждой модели

using EventsWebApplication.Core.DTOs;
using EventsWebApplication.Core.Interfaces.UseCases;
using EventsWebApplication.Core.Interfaces;
using EventsWebApplication.Core.Models;
using EventsWebApplication.Infrastructure.Services;
using Microsoft.AspNetCore.Http;

namespace EventsWebApplication.Infrastructure.UseCases
{
    public class EventUseCase : IEventUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ImageService _imageService;
        private readonly EventService _eventService;
        private readonly IMapper _mapper;

        public EventUseCase(IUnitOfWork unitOfWork, ImageService imageService, EventService eventService, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _imageService = imageService;
            _eventService = eventService;
            _mapper = mapper;
        }

        public async Task<IEnumerable<EventResponseDto>> GetAllEventsAsync()
        {
            var events = await _unitOfWork.Events.GetAllAsync();
            return _mapper.Map<IEnumerable<EventResponseDto>>(events);
        }

        public async Task<EventResponseDto> GetEventByIdAsync(int id)
        {
            var eventItem = await _unitOfWork.Events.GetEventByIdAsync(id);
            if (eventItem == null) throw new KeyNotFoundException("Событие не найдено");
            return _mapper.Map<EventResponseDto>(eventItem);
        }

        public async Task<EventResponseDto> GetEventByNameAsync(string name)
        {
            var eventItem = await _unitOfWork.Events.GetEventByTitleAsync(name);
            if (eventItem == null) throw new KeyNotFoundException("Событие не найдено");
            return _mapper.Map<EventResponseDto>(eventItem);
        }

        public async Task<EventResponseDto> AddEventAsync(EventRequestDto newEventDto)
        {
            var newEvent = _mapper.Map<Event>(newEventDto);
            await _unitOfWork.Events.AddAsync(newEvent);
            await _unitOfWork.CompleteAsync();
            return _mapper.Map<EventResponseDto>(newEvent);
        }

        public async Task UpdateEventAsync(int id, EventRequestDto updatedEventDto)
        {
            var eventToUpdate = await _unitOfWork.Events.GetEventByIdAsync(id);
            if (eventToUpdate == null) throw new KeyNotFoundException("Событие не найдено");

            var oldEvent = new Event
            {
                Title = eventToUpdate.Title,
                Description = eventToUpdate.Description,
                DateAndTime = eventToUpdate.DateAndTime,
                Location = eventToUpdate.Location,
                Category = eventToUpdate.Category,
                MaxParticipants = eventToUpdate.MaxParticipants
            };

            _mapper.Map(updatedEventDto, eventToUpdate);
            eventToUpdate.DateAndTime = eventToUpdate.DateAndTime.AddHours(3);

            var changesMessage = $"Событие {oldEvent.Title} было изменено. Изменения: ";
            if (eventToUpdate.Title != oldEvent.Title)
                changesMessage += $"Название было изменено с \"{oldEvent.Title}\" на \"{eventToUpdate.Title}\". ";
            if (eventToUpdate.Description != oldEvent.Description)
                changesMessage += $"Описание было изменено. ";
            if (eventToUpdate.DateAndTime != oldEvent.DateAndTime)
                changesMessage += $"Дата и время были изменены с \"{oldEvent.DateAndTime:dd.MM.yyyy HH:mm}\" на \"{eventToUpdate.DateAndTime:dd.MM.yyyy HH:mm}\". ";
            if (eventToUpdate.Location != oldEvent.Location)
                changesMessage += $"Место проведения было изменено с \"{oldEvent.Location}\" на \"{eventToUpdate.Location}\". ";
            if (eventToUpdate.Category != oldEvent.Category)
                changesMessage += $"Категория была изменена с \"{oldEvent.Category}\" на \"{eventToUpdate.Category}\". ";
            if (eventToUpdate.MaxParticipants != oldEvent.MaxParticipants)
                changesMessage += $"Максимальное количество участников было изменено с {oldEvent.MaxParticipants} на {eventToUpdate.MaxParticipants}. ";

            await _unitOfWork.Notifications.NotifyParticipantsAboutEventChange(
                await _unitOfWork.Participants.GetParticipantsByEventIdAsync(id),
                changesMessage);

            _unitOfWork.Events.Update(eventToUpdate);
            await _unitOfWork.CompleteAsync();
        }


        public async Task DeleteEventAsync(int id)
        {
            var eventToDelete = await _unitOfWork.Events.GetEventByIdAsync(id);
            if (eventToDelete == null) throw new KeyNotFoundException("Событие не найдено");

            await _unitOfWork.Events.DeleteAsync(id);
            await _unitOfWork.CompleteAsync();
        }

        public async Task<string> UploadEventImageAsync(int id, IFormFile image)
        {
            var eventToUpdate = await _unitOfWork.Events.GetEventByIdAsync(id);
            if (eventToUpdate == null) throw new KeyNotFoundException("Событие не найдено");

            var imagePath = await _imageService.SaveImageAsync(image);
            if (string.IsNullOrEmpty(imagePath)) throw new InvalidOperationException("Ошибка загрузки изображения");

            eventToUpdate.ImagePath = imagePath;
            await _unitOfWork.CompleteAsync();

            return imagePath;
        }

        public FileContentResult GetEventImage(int eventId)
        {
            var eventItem = _unitOfWork.Events.GetEventByIdAsync(eventId).Result;
            if (eventItem == null || string.IsNullOrEmpty(eventItem.ImagePath))
                throw new KeyNotFoundException("Изображение не найдено");

            var imagePath = _imageService.GetImagePath(eventItem.ImagePath);
            if (!File.Exists(imagePath)) throw new FileNotFoundException("Изображение не найдено на сервере");

            var image = File.ReadAllBytes(imagePath);
            var contentType = "image/" + Path.GetExtension(imagePath).TrimStart('.');
            return new FileContentResult(image, contentType);
        }

        public async Task<object> GetEventsByFilterAsync(EventFilterDto filter, int page, int pageSize)
        {
            var query = _unitOfWork.Events.GetEventsWithFilterQuery(filter);

            var totalCount = await query.CountAsync();
            var items = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new
            {
                items,
                totalPages = (int)Math.Ceiling(totalCount / (double)pageSize)
            };
        }

        public async Task<int> GetAvailableSeatsAsync(int id)
        {
            return await _eventService.GetAvailableSeatsAsync(id);
        }
    }
}
