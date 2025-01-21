using AutoMapper;
using EventsWebApplication.Core.DTOs.EventDTOs;
using EventsWebApplication.Core.Interfaces;

namespace EventsWebApplication.Application.UseCases.EventUseCases
{
    public class GetAllEventsUseCase : IUseCase<object, IEnumerable<EventResponseDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetAllEventsUseCase(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<EventResponseDto>> ExecuteAsync(object request)
        {
            var events = await _unitOfWork.Events.GetAllAsync();
            return _mapper.Map<IEnumerable<EventResponseDto>>(events);
        }
    }
}
