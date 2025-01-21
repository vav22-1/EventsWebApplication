using AutoMapper;
using EventsWebApplication.Core.DTOs.EventDTOs;
using EventsWebApplication.Core.Interfaces;

namespace EventsWebApplication.Application.UseCases.EventUseCases
{
    public class GetEventsByParticipantWithFilterUseCase : IUseCase<GetParticipantEventsRequestDto, PaginatedEventResponseDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetEventsByParticipantWithFilterUseCase(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PaginatedEventResponseDto> ExecuteAsync(GetParticipantEventsRequestDto request)
        {
            var (events, totalCount) = await _unitOfWork.Events.GetEventsByParticipantIdWithFilterAsync(request.ParticipantId, request.FilterAndPagination.Filter, request.FilterAndPagination.Page, request.FilterAndPagination.PageSize);

            return new PaginatedEventResponseDto
            {
                Items = _mapper.Map<IEnumerable<EventResponseDto>>(events),
                TotalPages = (int)Math.Ceiling(totalCount / 10.0)
            };
        }
    }
}
