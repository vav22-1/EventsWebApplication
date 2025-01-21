using AutoMapper;
using EventsWebApplication.Core.DTOs.EventDTOs;
using EventsWebApplication.Core.Interfaces;
using EventsWebApplication.Core.Models;

namespace EventsWebApplication.Application.UseCases.EventUseCases
{
    public class AddEventUseCase : IUseCase<EventRequestDto, EventResponseDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public AddEventUseCase(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<EventResponseDto> ExecuteAsync(EventRequestDto request)
        {
            var newEvent = _mapper.Map<Event>(request);
            await _unitOfWork.Events.AddAsync(newEvent);
            await _unitOfWork.CompleteAsync();
            return _mapper.Map<EventResponseDto>(newEvent);
        }
    }
}
