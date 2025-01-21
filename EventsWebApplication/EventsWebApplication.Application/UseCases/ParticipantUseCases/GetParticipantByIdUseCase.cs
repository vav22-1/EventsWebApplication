using AutoMapper;
using EventsWebApplication.Core.DTOs.ParticipantDTOs;
using EventsWebApplication.Core.Interfaces;

namespace EventsWebApplication.Application.UseCases.ParticipantUseCases
{
    public class GetParticipantByIdUseCase : IUseCase<GetParticipantByIdRequestDto, ParticipantResponseDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetParticipantByIdUseCase(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ParticipantResponseDto> ExecuteAsync(GetParticipantByIdRequestDto request)
        {
            var userId = int.Parse(request.User.Claims.FirstOrDefault(c => c.Type == "id")?.Value ?? "0");
            var userItem = await _unitOfWork.Users.GetByIdAsync(userId);

            if (userItem == null || userItem.ParticipantId != request.Id)
                throw new UnauthorizedAccessException("Нет доступа к данным этого участника.");

            var participant = await _unitOfWork.Participants.GetParticipantByIdAsync(request.Id);
            if (participant == null) throw new KeyNotFoundException("Участник не найден");

            return _mapper.Map<ParticipantResponseDto>(participant);
        }
    }
}
