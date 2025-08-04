using AptitudeTestApp.Data.Models;

namespace AptitudeTestApp.Application.DTOs;

public class StudentAnswerDto : BaseDto<Guid>
{
    public Guid SubmissionId { get; set; }
    public Guid QuestionId { get; set; }
    public Guid? SelectedOptionId { get; set; }
    public bool IsCorrect { get; set; } = false;
    public decimal PointsEarned { get; set; } = 0;
    public int TimeSpent { get; set; } = 0; // seconds
    public DateTime AnsweredAt { get; set; } = DateTime.Now;
    public virtual QuestionDto Question { get; set; } = null!;
    public virtual QuestionOptionDto? SelectedOption { get; set; }
}
