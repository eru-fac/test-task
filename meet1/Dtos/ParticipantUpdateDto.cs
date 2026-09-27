using System.ComponentModel.DataAnnotations;

namespace MeetingsApi.Dtos;

public sealed class ParticipantUpdateDto
{
    [Required]
    [MaxLength(120)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(160)]
    public string? Email { get; set; }

    [MaxLength(120)]
    public string? Position { get; set; }
}
