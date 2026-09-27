namespace MeetingsApi.Models;

public sealed class MeetingParticipant
{
    public int MeetingId { get; set; }

    public Meeting Meeting { get; set; } = null!;

    public int ParticipantId { get; set; }

    public Participant Participant { get; set; } = null!;
}
