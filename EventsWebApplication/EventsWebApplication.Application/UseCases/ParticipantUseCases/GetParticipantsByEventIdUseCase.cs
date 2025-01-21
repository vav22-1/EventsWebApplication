using AutoMapper;
using EventsWebApplication.Core.DTOs.ParticipantDTOs;
using EventsWebApplication.Core.Interfaces;

namespace EventsWebApplication.Application.UseCases.ParticipantUseCases
{
    public class GetParticipantsByEventIdUseCase : IUseCase<GetParticipantsByEventIdRequestDto, IEnumerable<ParticipantResponseDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetParticipantsByEventIdUseCase(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ParticipantResponseDto>> ExecuteAsync(GetParticipantsByEventIdRequestDto request)
        {
            var participants = await _unitOfWork.Participants.GetParticipantsByEventIdAsync(request.EventId);
            return _mapper.Map<IEnumerable<ParticipantResponseDto>>(participants);
        }
    }
}
