namespace EventsWebApplication.Core.DTOs.EventDTOs
{
    public class PaginatedEventRequestDto
    {
        public EventFilterDto? Filter;
        public int Page;
        public int PageSize;
    }
}
