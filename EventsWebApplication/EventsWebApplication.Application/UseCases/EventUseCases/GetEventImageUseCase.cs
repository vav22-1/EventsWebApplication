using EventsWebApplication.Core.DTOs.EventDTOs;
using EventsWebApplication.Core.Interfaces.Services;
using EventsWebApplication.Core.Interfaces;

namespace EventsWebApplication.Application.UseCases.EventUseCases
{
    public class GetEventImageUseCase : IUseCase<GetEventByIdDto, byte[]>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IImageService _imageService;

        public GetEventImageUseCase(IUnitOfWork unitOfWork, IImageService imageService)
        {
            _unitOfWork = unitOfWork;
            _imageService = imageService;
        }

        public async Task<byte[]> ExecuteAsync(GetEventByIdDto request)
        {
            var eventItem = await _unitOfWork.Events.GetEventByIdAsync(request.Id);
            if (eventItem == null || string.IsNullOrEmpty(eventItem.ImagePath))
                throw new KeyNotFoundException("Изображение не найдено");

            return _imageService.ReadImage(eventItem.ImagePath);
        }
    }
}
