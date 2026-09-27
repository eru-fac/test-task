namespace MeetingsApi.DTOs;
public class MeetingDetailDto { public int Id {get;set;} public string Title {get;set;}=""; public DateTime StartTime {get;set;} public string? RoomName {get;set;} public List<ParticipantDto> Participants {get;set;}=new(); }
