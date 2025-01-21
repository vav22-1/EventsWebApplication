using AutoMapper;
using EventsWebApplication.Core.DTOs.EventDTOs;
using EventsWebApplication.Core.Interfaces;

namespace EventsWebApplication.Application.UseCases.EventUseCases
{
    public class GetPaginatedEventsUseCase : IUseCase<PaginatedEventRequestDto, PaginatedEventResponseDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetPaginatedEventsUseCase(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PaginatedEventResponseDto> ExecuteAsync(PaginatedEventRequestDto request)
        {
            var (events, totalCount) = await _unitOfWork.Events.GetPaginatedEventsAsync(request.Filter,request.Page,request.PageSize);

            return new PaginatedEventResponseDto
            {
                Items = _mapper.Map<IEnumerable<EventResponseDto>>(events),
                TotalPages = (int)Math.Ceiling(totalCount / 10.0)
            };
        }
    }

}
