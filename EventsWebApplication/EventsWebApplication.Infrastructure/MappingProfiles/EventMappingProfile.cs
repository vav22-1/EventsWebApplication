//Профили маппера разбиты на разные файлы

using EventsWebApplication.Core.Models;
using EventsWebApplication.Core.DTOs.EventDTOs;

namespace EventsWebApplication.Infrastructure.MappingProfiles
{
    public class EventMappingProfile : Profile
    {
        public EventMappingProfile()
        {
            CreateMap<Event, EventRequestDto>().ReverseMap();
            CreateMap<Event, EventResponseDto>()
                .ForMember(dest => dest.ParticipantIds, opt => opt.MapFrom(src => src.EventParticipants.Select(ep => ep.ParticipantId)))
                .ReverseMap();
        }
    }
}
