using EventsWebApplication.Core.Models;

namespace EventsWebApplication.Application.DTOs.EventDTOs
{
    public class PaginatedEventRequestDto
    {
        public EventFilter? Filter;
        public int Page;
        public int PageSize;
    }
}
