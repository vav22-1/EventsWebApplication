namespace EventsWebApplication.Core.DTOs.EventDTOs
{
    public class PaginatedEventResponseDto
    {
        public IEnumerable<EventResponseDto> Items { get; set; }
        public int TotalPages { get; set; }
    }
}
