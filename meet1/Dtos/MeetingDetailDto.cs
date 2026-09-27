namespace MeetingsApi.Dtos;

public sealed class MeetingDetailDto
{
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string? Description { get; set; }

    public DateTime StartTime { get; set; }

    public DateTime EndTime { get; set; }

    public RoomDto? Room { get; set; }

    public List<ParticipantDto> Participants { get; set; } = new();
}
