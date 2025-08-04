using System.ComponentModel.DataAnnotations;

namespace AptitudeTestApp.Application.DTOs;

public class AntiCheatLogDto : BaseDto<Guid>
{
    public Guid SubmissionId { get; set; }

    [Required, MaxLength(100)]
    public string EventType { get; set; } = string.Empty;

    public string? EventDetails { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.Now;
}
