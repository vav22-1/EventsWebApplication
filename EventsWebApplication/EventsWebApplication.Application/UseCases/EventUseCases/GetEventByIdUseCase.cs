using AutoMapper;
using EventsWebApplication.Core.DTOs.EventDTOs;
using EventsWebApplication.Core.Interfaces;

namespace EventsWebApplication.Application.UseCases.EventUseCases
{
    public class GetEventByIdUseCase : IUseCase<GetEventByIdDto, EventResponseDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetEventByIdUseCase(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<EventResponseDto> ExecuteAsync(GetEventByIdDto request)
        {
            var eventItem = await _unitOfWork.Events.GetEventByIdAsync(request.Id);
            if (eventItem == null) throw new KeyNotFoundException("Событие не найдено");
            return _mapper.Map<EventResponseDto>(eventItem);
        }
    }
}
