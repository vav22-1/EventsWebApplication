using EventsWebApplication.Core.DTOs;
using System.Security.Claims;

namespace EventsWebApplication.Core.Interfaces.UseCases
{
    public interface IParticipantUseCase
    {
        Task<IEnumerable<ParticipantResponseDto>> GetParticipantsByEventIdAsync(int eventId);
        Task<ParticipantResponseDto> GetParticipantByIdAsync(int id, ClaimsPrincipal user);
        Task RegisterParticipantAsync(int eventId, ClaimsPrincipal user);
        Task UpdateParticipantAsync(int id, ParticipantRequestDto participantDto, ClaimsPrincipal user);
        Task DeleteParticipantFromEventAsync(int eventId, int id, ClaimsPrincipal user);
    }
}
