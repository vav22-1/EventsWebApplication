using System.Security.Claims;

namespace EventsWebApplication.Core.DTOs.ParticipantDTOs
{
    public class UpdateParticipantRequestDto
    {
        public int Id { get; set; }
        public ParticipantRequestDto ParticipantDto { get; set; }
        public ClaimsPrincipal User { get; set; }
    }
}
