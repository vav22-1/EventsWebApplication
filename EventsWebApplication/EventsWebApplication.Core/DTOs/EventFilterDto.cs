namespace EventsWebApplication.Core.DTOs
{
    public class EventFilterDto
    {
        public string? Title { get; set; }
        public DateTime? FilterDate { get; set; }
        public string? FilterLocation { get; set; }
        public string? FilterCategory { get; set; }
    }
}
