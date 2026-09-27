using AutoMapper;
using MeetingsApi.Dtos;
using MeetingsApi.Models;

namespace MeetingsApi.Mapping;

public sealed class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Room, RoomDto>();

        CreateMap<Participant, ParticipantDto>();

        CreateMap<MeetingParticipant, ParticipantDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Participant.Id))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Participant.Name))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Participant.Email))
            .ForMember(dest => dest.Position, opt => opt.MapFrom(src => src.Participant.Position));

        CreateMap<Meeting, MeetingDto>()
            .ForMember(dest => dest.RoomName, opt => opt.MapFrom(src => src.Room != null ? src.Room.Name : null))
            .ForMember(dest => dest.ParticipantsCount, opt => opt.MapFrom(src => src.Participants.Count));

        CreateMap<Meeting, MeetingDetailDto>()
            .ForMember(dest => dest.Room, opt => opt.MapFrom(src => src.Room))
            .ForMember(dest => dest.Participants, opt => opt.MapFrom(src => src.Participants));

        CreateMap<MeetingCreateDto, Meeting>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Room, opt => opt.Ignore())
            .ForMember(dest => dest.Participants, opt => opt.Ignore());

        CreateMap<MeetingUpdateDto, Meeting>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Room, opt => opt.Ignore())
            .ForMember(dest => dest.Participants, opt => opt.Ignore());

        CreateMap<ParticipantCreateDto, Participant>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Meetings, opt => opt.Ignore());

        CreateMap<ParticipantUpdateDto, Participant>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Meetings, opt => opt.Ignore());

        CreateMap<RoomCreateDto, Room>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Meetings, opt => opt.Ignore());

        CreateMap<RoomUpdateDto, Room>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Meetings, opt => opt.Ignore());
    }
}
