using System.ComponentModel.DataAnnotations;

namespace MeetingsApi.Dtos;

public sealed class MeetingUpdateDto
{
    [Required]
    [MaxLength(120)]
    public string Title { get; set; } = string.Empty;

    [MaxLength(500)]
    public string? Description { get; set; }

    [Required]
    public DateTime StartTime { get; set; }

    [Required]
    public DateTime EndTime { get; set; }

    public int? RoomId { get; set; }

    public List<int> ParticipantIds { get; set; } = new();
}
