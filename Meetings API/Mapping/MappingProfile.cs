using AutoMapper;
using MeetingsApi.Models;
using MeetingsApi.DTOs;

namespace MeetingsApi.Mapping;
public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Participant, ParticipantDto>();
        CreateMap<Meeting, MeetingDto>().ForMember(d=>d.ParticipantsCount,o=>o.MapFrom(s=>s.Participants.Count));
        CreateMap<Meeting, MeetingDetailDto>().ForMember(d=>d.RoomName,o=>o.MapFrom(s=>s.Room==null?null:s.Room.Name));
        CreateMap<MeetingCreateDto, Meeting>().ForMember(d=>d.Participants,o=>o.Ignore()).ForMember(d=>d.Room,o=>o.Ignore());
        CreateMap<MeetingUpdateDto, Meeting>().ForMember(d=>d.Participants,o=>o.Ignore()).ForMember(d=>d.Room,o=>o.Ignore());
    }
}
