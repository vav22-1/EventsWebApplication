namespace EventsWebApplication.Core.DTOs.EventDTOs
{
    public class EventRequestDto
    {
        public string Title { get; set; }
        public string? Description { get; set; }
        public DateTime DateAndTime { get; set; }
        public string Location { get; set; }
        public string Category { get; set; }
        public int MaxParticipants { get; set; }
    }
}
