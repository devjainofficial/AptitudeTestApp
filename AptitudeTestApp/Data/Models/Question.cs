using AptitudeTestApp.Shared.Enums;
using System.ComponentModel.DataAnnotations;

namespace AptitudeTestApp.Data.Models;
public class Question : BaseEntity<Guid>
{
    public Guid CategoryId { get; set; }

    [Required]
    public string QuestionText { get; set; } = string.Empty;

    [MaxLength(50)]
    public string QuestionType { get; set; } = "MultipleChoice";

    public QuestionDifficulty DifficultyLevel { get; set; } = QuestionDifficulty.Easy;
    public int Points { get; set; } = 1;
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public string? CreatedBy { get; set; }

    // Navigation Properties
    public virtual QuestionCategory Category { get; set; } = null!;
    public virtual ICollection<QuestionOption> Options { get; set; } = new List<QuestionOption>();
    public virtual ICollection<TestSessionQuestion> TestSessionQuestions { get; set; } = new List<TestSessionQuestion>();
    public virtual ICollection<StudentAnswer> StudentAnswers { get; set; } = new List<StudentAnswer>();
}

