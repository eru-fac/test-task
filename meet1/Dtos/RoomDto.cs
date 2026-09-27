namespace MeetingsApi.Dtos;

public sealed class RoomDto
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string? Location { get; set; }

    public int Capacity { get; set; }
}
