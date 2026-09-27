using MeetingsApi.Models;

namespace MeetingsApi.Data;

public static class SeedData
{
    public static void Initialize(Data.AppDbContext db)
    {
        if (db.Participants.Any()) return;
        var p = new[] {
            new Participant { Name="Іван Петренко", Email="ivan@example.com" },
            new Participant { Name="Олена Коваль", Email="olena@example.com" },
            new Participant { Name="Андрій Мельник", Email="andrii@example.com" }
        };
        var r = new[] { new Room {Name="Кімната 101"}, new Room {Name="Кімната 202"} };
        db.Participants.AddRange(p); db.Rooms.AddRange(r); db.SaveChanges();
        db.Meetings.AddRange(
            new Meeting { Title="Планування проєкту", StartTime=DateTime.UtcNow.AddDays(1), RoomId=r[0].Id, Participants=new List<Participant>{p[0],p[1]} },
            new Meeting { Title="Обговорення задач", StartTime=DateTime.UtcNow.AddDays(2), RoomId=r[1].Id, Participants=new List<Participant>{p[1],p[2]} }
        );
        db.SaveChanges();
    }
}
