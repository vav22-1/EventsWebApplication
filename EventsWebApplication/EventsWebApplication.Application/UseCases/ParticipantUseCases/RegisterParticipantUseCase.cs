using EventsWebApplication.Application.DTOs.ParticipantDTOs;

namespace EventsWebApplication.Application.UseCases.ParticipantUseCases
{
    public class RegisterParticipantUseCase : IUseCase<RegisterParticipantRequestDto, Unit>
    {
        private readonly IUnitOfWork _unitOfWork;

        public RegisterParticipantUseCase(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> ExecuteAsync(RegisterParticipantRequestDto request)
        {
            var userId = int.Parse(request.User.Claims.FirstOrDefault(c => c.Type == "id")?.Value ?? "0");
            var userItem = await _unitOfWork.Users.GetByIdAsync(userId);

            if (userItem == null || userItem.ParticipantId == null)
                throw new KeyNotFoundException("Пользователь не найден или нет участника.");

            var participantId = userItem.ParticipantId.Value;
            await _unitOfWork.Participants.AddParticipantToEventAsync(request.EventId, participantId);
            await _unitOfWork.CompleteAsync();

            return new Unit();
        }
    }
}
