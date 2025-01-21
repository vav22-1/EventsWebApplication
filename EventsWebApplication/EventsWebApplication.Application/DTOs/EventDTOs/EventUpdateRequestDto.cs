namespace EventsWebApplication.Application.DTOs.EventDTOs
{
    public class EventUpdateRequestDto
    {
        public int Id { get; set; }
        public EventRequestDto UpdatedEventDto {  get; set; }
    }
}
