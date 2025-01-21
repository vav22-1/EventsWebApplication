using System.Security.Claims;

namespace EventsWebApplication.Core.DTOs.ParticipantDTOs
{
    public class DeleteParticipantFromEventRequestDto
    {
        public int EventId { get; set; }
        public int ParticipantId { get; set; }
        public ClaimsPrincipal User { get; set; }
    }
}
