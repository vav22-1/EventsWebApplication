using EventsWebApplication.Core.DTOs.ParticipantDTOs;
using EventsWebApplication.Core.Interfaces;

namespace EventsWebApplication.Application.UseCases.ParticipantUseCases
{
    public class DeleteParticipantFromEventUseCase : IUseCase<DeleteParticipantFromEventRequestDto, Unit>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteParticipantFromEventUseCase(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> ExecuteAsync(DeleteParticipantFromEventRequestDto request)
        {
            var userId = int.Parse(request.User.Claims.FirstOrDefault(c => c.Type == "id")?.Value ?? "0");
            var userItem = await _unitOfWork.Users.GetByIdAsync(userId);
            if (userItem == null || userItem.ParticipantId != request.ParticipantId)
                throw new UnauthorizedAccessException("Нет доступа к данным этого участника.");

            await _unitOfWork.Participants.RemoveParticipantFromEventAsync(request.EventId, request.ParticipantId);
            await _unitOfWork.CompleteAsync();

            return new Unit();
        }
    }
}
