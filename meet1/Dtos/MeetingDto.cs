namespace MeetingsApi.Dtos;

public sealed class MeetingDto
{
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public DateTime StartTime { get; set; }

    public DateTime EndTime { get; set; }

    public string? RoomName { get; set; }

    public int ParticipantsCount { get; set; }
}
