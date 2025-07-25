namespace AptitudeTestApp.Data.Models;

public class StudentAnswer : BaseEntity<Guid>
{
    public Guid SubmissionId { get; set; }
    public Guid QuestionId { get; set; }
    public Guid? SelectedOptionId { get; set; }
    public bool IsCorrect { get; set; } = false;
    public decimal PointsEarned { get; set; } = 0;
    public int TimeSpent { get; set; } = 0; // seconds
    public DateTime AnsweredAt { get; set; } = DateTime.UtcNow;

    // Navigation Properties
    public virtual StudentSubmission Submission { get; set; } = null!;
    public virtual Question Question { get; set; } = null!;
    public virtual QuestionOption? SelectedOption { get; set; }
}
