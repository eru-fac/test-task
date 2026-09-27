using System.ComponentModel.DataAnnotations;

namespace MeetingsApi.Dtos;

public sealed class RoomCreateDto
{
    [Required]
    [MaxLength(120)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(160)]
    public string? Location { get; set; }

    [Range(1, 500)]
    public int Capacity { get; set; }
}
