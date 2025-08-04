using System.ComponentModel.DataAnnotations;

namespace AptitudeTestApp.Data.Models;
public class AntiCheatLog : BaseEntity<Guid>
{
    public Guid SubmissionId { get; set; }

    [Required, MaxLength(100)]
    public string EventType { get; set; } = string.Empty;

    public string? EventDetails { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.Now;

    // Navigation Properties
    public virtual StudentSubmission Submission { get; set; } = null!;
}