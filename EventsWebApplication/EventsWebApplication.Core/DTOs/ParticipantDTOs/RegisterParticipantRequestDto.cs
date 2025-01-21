using System.Security.Claims;

namespace EventsWebApplication.Core.DTOs.ParticipantDTOs
{
    public class RegisterParticipantRequestDto
    {
        public int EventId { get; set; }
        public ClaimsPrincipal User { get; set; }
    }
}
