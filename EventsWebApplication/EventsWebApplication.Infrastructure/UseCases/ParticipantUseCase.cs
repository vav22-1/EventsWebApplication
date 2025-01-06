//Добавлены отдельные use cases для каждой модели

using EventsWebApplication.Core.DTOs;
using EventsWebApplication.Core.Interfaces;
using EventsWebApplication.Core.Interfaces.UseCases;
using System.Security.Claims;

namespace EventsWebApplication.Infrastructure.UseCases
{
    public class ParticipantUseCase : IParticipantUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ParticipantUseCase(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ParticipantResponseDto>> GetParticipantsByEventIdAsync(int eventId)
        {
            var participants = await _unitOfWork.Participants.GetParticipantsByEventIdAsync(eventId);
            return _mapper.Map<IEnumerable<ParticipantResponseDto>>(participants);
        }

        public async Task<ParticipantResponseDto> GetParticipantByIdAsync(int id, ClaimsPrincipal user)
        {
            var userId = int.Parse(user.Claims.FirstOrDefault(c => c.Type == "id")?.Value ?? "0");
            var userItem = await _unitOfWork.Users.GetUserByIdAsync(userId);
            if (userItem == null || userItem.ParticipantId != id)
                throw new UnauthorizedAccessException("Нет доступа к данным этого участника.");

            var participant = await _unitOfWork.Participants.GetParticipantByIdAsync(id);
            if (participant == null) throw new KeyNotFoundException("Участник не найден");

            return _mapper.Map<ParticipantResponseDto>(participant);
        }

        public async Task RegisterParticipantAsync(int eventId, ClaimsPrincipal user)
        {
            var userId = int.Parse(user.Claims.FirstOrDefault(c => c.Type == "id")?.Value ?? "0");
            var userItem = await _unitOfWork.Users.GetUserByIdAsync(userId);
            if (userItem == null || userItem.ParticipantId == null)
                throw new KeyNotFoundException("Пользователь не найден или нет участника.");

            var participantId = userItem.ParticipantId.Value;
            await _unitOfWork.Participants.AddParticipantToEventAsync(eventId, participantId);
            await _unitOfWork.CompleteAsync();
        }

        public async Task UpdateParticipantAsync(int id, ParticipantRequestDto participantDto, ClaimsPrincipal user)
        {
            var userId = int.Parse(user.Claims.FirstOrDefault(c => c.Type == "id")?.Value ?? "0");
            var userItem = await _unitOfWork.Users.GetUserByIdAsync(userId);
            if (userItem == null || userItem.ParticipantId != id)
                throw new UnauthorizedAccessException("Нет доступа к данным этого участника.");

            var participant = await _unitOfWork.Participants.GetParticipantByIdAsync(id);
            if (participant == null) throw new KeyNotFoundException("Участник не найден");

            _mapper.Map(participantDto, participant);
            _unitOfWork.Participants.Update(participant);
            await _unitOfWork.CompleteAsync();
        }

        public async Task DeleteParticipantFromEventAsync(int eventId, int id, ClaimsPrincipal user)
        {
            var userId = int.Parse(user.Claims.FirstOrDefault(c => c.Type == "id")?.Value ?? "0");
            var userItem = await _unitOfWork.Users.GetUserByIdAsync(userId);
            if (userItem == null || userItem.ParticipantId != id)
                throw new UnauthorizedAccessException("Нет доступа к данным этого участника.");

            await _unitOfWork.Participants.RemoveParticipantFromEventAsync(eventId, id);
            await _unitOfWork.CompleteAsync();
        }
    }
}
