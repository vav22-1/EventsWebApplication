using EventsWebApplication.Core.Interfaces.Repositories;

namespace EventsWebApplication.Infrastructure.Services
{
    public class EventService
    {
        private readonly IEventRepository _eventRepository;

        public EventService(IEventRepository eventRepository)
        {
            _eventRepository = eventRepository;
        }

        public async Task<int> GetAvailableSeatsAsync(int eventId)
        {
            var eventEntity = await _eventRepository.GetEventByIdAsync(eventId);
            if (eventEntity != null)
            {
                var currentParticipants = await _eventRepository.GetCurrentParticipantsAsync(eventId);
                return eventEntity.MaxParticipants - currentParticipants;
            }
            return 0;
        }
    }
}
