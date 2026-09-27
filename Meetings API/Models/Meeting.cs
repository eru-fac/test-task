namespace MeetingsApi.Models;
public class Meeting
{
    public int Id {get;set;}
    public string Title {get;set;} = "";
    public DateTime StartTime {get;set;}
    public int? RoomId {get;set;}
    public Room? Room {get;set;}
    public ICollection<Participant> Participants {get;set;} = new List<Participant>();
}
