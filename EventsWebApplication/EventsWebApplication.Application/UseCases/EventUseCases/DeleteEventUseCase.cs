using EventsWebApplication.Application.DTOs.EventDTOs;

namespace EventsWebApplication.Application.UseCases.EventUseCases
{
    public class DeleteEventUseCase : IUseCase<DeleteEventDto, string>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteEventUseCase(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<string> ExecuteAsync(DeleteEventDto request)
        {
            var eventToDelete = await _unitOfWork.Events.GetEventByIdAsync(request.Id);
            if (eventToDelete == null) throw new KeyNotFoundException("Событие не найдено");

            _unitOfWork.Events.Delete(eventToDelete);
            await _unitOfWork.CompleteAsync();

            return "Событие удалено.";
        }
    }
}
