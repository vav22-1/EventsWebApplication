using AutoMapper;
using EventsWebApplication.Core.Interfaces;
using EventsWebApplication.Core.DTOs.EventDTOs;
using EventsWebApplication.Core.Models;

namespace EventsWebApplication.Application.UseCases.EventUseCases
{
    public class UpdateEventUseCase : IUseCase<EventUpdateRequestDto, string>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UpdateEventUseCase(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<string> ExecuteAsync(EventUpdateRequestDto request)
        {
            var eventToUpdate = await _unitOfWork.Events.GetEventByIdAsync(request.Id);
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

            _mapper.Map(request.UpdatedEventDto, eventToUpdate);
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

            await _unitOfWork.Notifications.NotifyParticipantsAboutEventChange(await _unitOfWork.Participants.GetParticipantsByEventIdAsync(request.Id), changesMessage);

            _unitOfWork.Events.Update(eventToUpdate);
            await _unitOfWork.CompleteAsync();

            return "Событие обновлено успешно.";
        }
    }
}
