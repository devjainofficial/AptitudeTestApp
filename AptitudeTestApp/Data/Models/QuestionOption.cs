using System.ComponentModel.DataAnnotations;

namespace AptitudeTestApp.Data.Models;

public class QuestionOption : BaseEntity<Guid>
{
    public Guid QuestionId { get; set; }

    [Required]
    public string OptionText { get; set; } = string.Empty;

    public bool IsCorrect { get; set; } = false;
    public int DisplayOrder { get; set; } = 0;

    // Navigation Properties
    public virtual Question Question { get; set; } = null!;
    public virtual ICollection<StudentAnswer> StudentAnswers { get; set; } = new List<StudentAnswer>();
}