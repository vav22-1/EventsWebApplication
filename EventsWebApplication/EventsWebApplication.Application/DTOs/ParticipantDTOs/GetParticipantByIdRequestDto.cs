using System.Security.Claims;

namespace EventsWebApplication.Application.DTOs.ParticipantDTOs
{
    public class GetParticipantByIdRequestDto
    {
        public int Id { get; set; }
        public ClaimsPrincipal User { get; set; }
    }
}
