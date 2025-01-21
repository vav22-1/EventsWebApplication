using EventsWebApplication.Core.Interfaces.Services;
using EventsWebApplication.Core.Interfaces;
using EventsWebApplication.Core.DTOs.EventDTOs;

namespace EventsWebApplication.Application.UseCases.EventUseCases
{
    public class UploadEventImageUseCase : IUseCase<UploadImageRequestDto, string>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IImageService _imageService;

        public UploadEventImageUseCase(IUnitOfWork unitOfWork, IImageService imageService)
        {
            _unitOfWork = unitOfWork;
            _imageService = imageService;
        }

        public async Task<string> ExecuteAsync(UploadImageRequestDto request)
        {
            var eventToUpdate = await _unitOfWork.Events.GetEventByIdAsync(request.EventId);
            if (eventToUpdate == null) throw new KeyNotFoundException("Событие не найдено");

            var imagePath = await _imageService.SaveImageAsync(request.ImageData);
            eventToUpdate.ImagePath = imagePath;

            await _unitOfWork.CompleteAsync();
            return imagePath;
        }
    }
}
