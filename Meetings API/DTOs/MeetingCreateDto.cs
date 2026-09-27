namespace MeetingsApi.DTOs;
public class MeetingCreateDto { public string Title {get;set;}=""; public DateTime StartTime {get;set;} public int? RoomId {get;set;} public List<int> ParticipantIds {get;set;}=new(); }
