namespace MeetingsApi.Dtos;

public sealed class ParticipantDto
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string? Email { get; set; }

    public string? Position { get; set; }
}
