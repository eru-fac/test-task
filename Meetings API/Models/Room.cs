namespace MeetingsApi.Models;
public class Room
{
    public int Id {get;set;}
    public string Name {get;set;} = "";
    public ICollection<Meeting> Meetings {get;set;} = new List<Meeting>();
}
