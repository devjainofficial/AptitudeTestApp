using System.ComponentModel.DataAnnotations;

namespace AptitudeTestApp.Data.Models;

public class TestSession : BaseEntity<Guid>
{
    public Guid UniversityId { get; set; }

    [Required, MaxLength(200)]
    public string TestName { get; set; } = string.Empty;

    [MaxLength(1000)]
    public string? Description { get; set; }

    public int TimeLimit { get; set; } // in minutes
    public int TotalQuestions { get; set; }
    public decimal PassingScore { get; set; } = 60.00m;
    public int MaxTabSwitches { get; set; } = 3;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }

    [Required, MaxLength(100)]
    public string Token { get; set; } = string.Empty;

    public bool IsActive { get; set; } = true;
    public string? CreatedBy { get; set; }

    // Navigation Properties
    public virtual University University { get; set; } = null!;
    public virtual ICollection<TestSessionQuestion> TestSessionQuestions { get; set; } = new List<TestSessionQuestion>();
    public virtual ICollection<StudentSubmission> StudentSubmissions { get; set; } = new List<StudentSubmission>();
}
