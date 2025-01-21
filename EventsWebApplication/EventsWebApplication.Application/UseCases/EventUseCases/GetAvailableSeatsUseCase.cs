using EventsWebApplication.Application.DTOs.EventDTOs;

namespace EventsWebApplication.Application.UseCases.EventUseCases
{
    public class GetAvailableSeatsUseCase : IUseCase<GetEventByIdDto, int>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetAvailableSeatsUseCase(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<int> ExecuteAsync(GetEventByIdDto request)
        {
            var eventEntity = await _unitOfWork.Events.GetEventByIdAsync(request.Id);
            if (eventEntity != null)
            {
                var currentParticipants = await _unitOfWork.Events.GetCurrentParticipantsAsync(request.Id);
                return eventEntity.MaxParticipants - currentParticipants;
            }
            return 0;
        }
    }
}
