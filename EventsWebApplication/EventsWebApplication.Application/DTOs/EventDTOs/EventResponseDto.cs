namespace EventsWebApplication.Application.DTOs.EventDTOs
{
    public class EventResponseDto
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public DateTime DateAndTime { get; set; }
        public string Location { get; set; }
        public string Category { get; set; }
        public int MaxParticipants { get; set; }
        public string ImagePath { get; set; }
        public List<int> ParticipantIds { get; set; } = new();
    }
}
