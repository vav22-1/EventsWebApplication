using AutoMapper;
using EventsWebApplication.Core.DTOs.ParticipantDTOs;
using EventsWebApplication.Core.Interfaces;

namespace EventsWebApplication.Application.UseCases
{
    public class UpdateParticipantUseCase : IUseCase<UpdateParticipantRequestDto, Unit>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UpdateParticipantUseCase(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Unit> ExecuteAsync(UpdateParticipantRequestDto request)
        {
            var userId = int.Parse(request.User.Claims.FirstOrDefault(c => c.Type == "id")?.Value ?? "0");
            var userItem = await _unitOfWork.Users.GetByIdAsync(userId);
            if (userItem == null || userItem.ParticipantId != request.Id)
                throw new UnauthorizedAccessException("Нет доступа к данным этого участника.");

            var participant = await _unitOfWork.Participants.GetParticipantByIdAsync(request.Id);
            if (participant == null) throw new KeyNotFoundException("Участник не найден");

            _mapper.Map(request.ParticipantDto, participant);
            _unitOfWork.Participants.Update(participant);
            await _unitOfWork.CompleteAsync();

            return new Unit();
        }
    }
}
