namespace EventsWebApplication.Application.DTOs.EventDTOs
{
    public class GetParticipantEventsRequestDto
    {
        public int ParticipantId { get; set; }
        public PaginatedEventRequestDto FilterAndPagination { get; set; }
    }
}
