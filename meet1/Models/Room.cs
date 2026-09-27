using System.ComponentModel.DataAnnotations;

namespace MeetingsApi.Models;

public sealed class Room
{
    public int Id { get; set; }

    [MaxLength(120)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(160)]
    public string? Location { get; set; }

    public int Capacity { get; set; }

    public List<Meeting> Meetings { get; set; } = new();
}
