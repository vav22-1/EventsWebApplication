namespace EventsWebApplication.Application.DTOs.ParticipantDTOs
{
    public class ParticipantResponseDto
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? Email { get; set; }
        public List<int> EventIds { get; set; } = new();
    }
}
