using System.ComponentModel.DataAnnotations;

namespace AptitudeTestApp.Data.Models;

public class QuestionCategory : BaseEntity<Guid>
{
    [Required, MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(500)]
    public string? Description { get; set; }

    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation Properties
    public virtual ICollection<Question> Questions { get; set; } = new List<Question>();
}