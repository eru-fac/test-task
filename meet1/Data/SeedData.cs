using MeetingsApi.Models;
using Microsoft.EntityFrameworkCore;

namespace MeetingsApi.Data;

public static class SeedData
{
    public static async Task SeedAsync(AppDbContext db)
    {
        if (await db.Meetings.AnyAsync())
        {
            return;
        }

        var room1 = new Room
        {
            Name = "Room 101",
            Location = "Main office",
            Capacity = 8
        };

        var room2 = new Room
        {
            Name = "Online",
            Location = "Google Meet",
            Capacity = 50
        };

        var participant1 = new Participant
        {
            Name = "Kirill Malik",
            Email = "kirill@example.com",
            Position = "Frontend Developer"
        };

        var participant2 = new Participant
        {
            Name = "Anna Smith",
            Email = "anna@example.com",
            Position = "Project Manager"
        };

        var participant3 = new Participant
        {
            Name = "Mark Johnson",
            Email = "mark@example.com",
            Position = "Designer"
        };

        var meeting1 = new Meeting
        {
            Title = "Project planning",
            Description = "Short meeting about project tasks",
            StartTime = DateTime.Today.AddHours(10),
            EndTime = DateTime.Today.AddHours(11),
            Room = room1
        };

        meeting1.Participants.Add(new MeetingParticipant { Meeting = meeting1, Participant = participant1 });
        meeting1.Participants.Add(new MeetingParticipant { Meeting = meeting1, Participant = participant2 });

        var meeting2 = new Meeting
        {
            Title = "Design review",
            Description = "Review of new screens",
            StartTime = DateTime.Today.AddDays(1).AddHours(13),
            EndTime = DateTime.Today.AddDays(1).AddHours(14),
            Room = room2
        };

        meeting2.Participants.Add(new MeetingParticipant { Meeting = meeting2, Participant = participant2 });
        meeting2.Participants.Add(new MeetingParticipant { Meeting = meeting2, Participant = participant3 });

        db.Meetings.AddRange(meeting1, meeting2);
        await db.SaveChangesAsync();
    }
}
