using AptitudeTestApp.Shared.Enums;
using System.ComponentModel.DataAnnotations;

namespace AptitudeTestApp.Data.Models;
public class StudentSubmission : BaseEntity<Guid>
{
    public Guid TestSessionId { get; set; }

    [Required, MaxLength(200)]
    public string StudentName { get; set; } = string.Empty;

    [Required, MaxLength(255)]
    public string StudentEmail { get; set; } = string.Empty;

    public DateTime StartTime { get; set; }
    public DateTime? EndTime { get; set; }
    public int? TimeTaken { get; set; } // in seconds
    public decimal TotalScore { get; set; } = 0;
    public decimal MaxPossibleScore { get; set; } = 0;
    public decimal PercentageScore { get; set; } = 0;

    [MaxLength(50)]
    public TestStatus Status { get; set; } = TestStatus.InProgress;

    public int TabSwitchCount { get; set; } = 0;
    public bool IsDisqualified { get; set; } = false;

    [MaxLength(500)]
    public string? DisqualificationReason { get; set; }

    [MaxLength(1000)]
    public string? BrowserInfo { get; set; }

    [MaxLength(45)]
    public string? IpAddress { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation Properties
    public virtual TestSession TestSession { get; set; } = null!;
    public virtual ICollection<StudentAnswer> StudentAnswers { get; set; } = new List<StudentAnswer>();
    public virtual ICollection<AntiCheatLog> AntiCheatLogs { get; set; } = new List<AntiCheatLog>();
}