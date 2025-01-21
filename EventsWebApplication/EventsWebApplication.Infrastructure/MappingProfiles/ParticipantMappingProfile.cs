//Профили маппера разбиты на разные файлы

using EventsWebApplication.Core.Models;
using EventsWebApplication.Core.DTOs.ParticipantDTOs;

namespace EventsWebApplication.Infrastructure.MappingProfiles
{
    public class ParticipantMappingProfile : Profile
    {
        public ParticipantMappingProfile()
        {
            CreateMap<Participant, ParticipantRequestDto>().ReverseMap();
            CreateMap<Participant, ParticipantResponseDto>()
                .ForMember(dest => dest.EventIds, opt => opt.MapFrom(src => src.EventParticipants.Select(ep => ep.EventId)))
                .ReverseMap();
        }
    }
}
