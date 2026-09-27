using System.ComponentModel.DataAnnotations;

namespace MeetingsApi.Models;

public sealed class Participant
{
    public int Id { get; set; }

    [MaxLength(120)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(160)]
    public string? Email { get; set; }

    [MaxLength(120)]
    public string? Position { get; set; }

    public List<MeetingParticipant> Meetings { get; set; } = new();
}
