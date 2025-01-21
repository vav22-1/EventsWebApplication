using EventsWebApplication.Application.DTOs.EventDTOs;

namespace EventsWebApplication.Application.UseCases.EventUseCases
{
    public class GetEventByNameUseCase : IUseCase<GetEventByNameDto, EventResponseDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetEventByNameUseCase(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<EventResponseDto> ExecuteAsync(GetEventByNameDto request)
        {
            var eventItem = await _unitOfWork.Events.GetEventByTitleAsync(request.Name);
            if (eventItem == null) throw new KeyNotFoundException("Событие не найдено");
            return _mapper.Map<EventResponseDto>(eventItem);
        }
    }
}
