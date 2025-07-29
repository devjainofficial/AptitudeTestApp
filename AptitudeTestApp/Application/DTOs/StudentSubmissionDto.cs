using AptitudeTestApp.Shared.Enums;

namespace AptitudeTestApp.Application.DTOs;

public class StudentSubmissionDto : BaseDto<Guid>
{
    public Guid TestSessionId { get; set; }
    public string StudentName { get; set; } = string.Empty;
    public string StudentEmail { get; set; } = string.Empty;
    public DateTime StartTime { get; set; }
    public DateTime? EndTime { get; set; }
    public int? TimeTaken { get; set; }
    public decimal TotalScore { get; set; } = 0;
    public decimal MaxPossibleScore { get; set; } = 0;
    public decimal PercentageScore { get; set; } = 0;
    public TestStatus Status { get; set; } = TestStatus.InProgress;
    public int TabSwitchCount { get; set; } = 0;
    public bool IsDisqualified { get; set; } = false;
    public string? DisqualificationReason { get; set; }
    public string? BrowserInfo { get; set; }
    public string? IpAddress { get; set; }

    public virtual TestSessionDto TestSession { get; set; } = null!;

}
