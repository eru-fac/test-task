using System.ComponentModel.DataAnnotations;

namespace MeetingsApi.Models;

public sealed class Meeting
{
    public int Id { get; set; }

    [MaxLength(120)]
    public string Title { get; set; } = string.Empty;

    [MaxLength(500)]
    public string? Description { get; set; }

    public DateTime StartTime { get; set; }

    public DateTime EndTime { get; set; }

    public int? RoomId { get; set; }

    public Room? Room { get; set; }

    public List<MeetingParticipant> Participants { get; set; } = new();
}
