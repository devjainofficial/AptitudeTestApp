using System.ComponentModel.DataAnnotations;

namespace AptitudeTestApp.Application.DTOs;

public class CreateTestSessionDto : BaseDto<Guid>
{
    [Required(ErrorMessage = "The Test Name field is required."), MaxLength(200)]
    public string TestName { get; set; } = string.Empty;

    public string? Description { get; set; }

    [Required]
    public Guid UniversityId { get; set; }

    [Range(10, 480)] // 10 minutes to 8 hours
    public int TimeLimit { get; set; } = 60;

    public int TotalQuestions { get; set; }

    [Range(0, 100)]
    public decimal PassingScore { get; set; } = 60.00m;

    [Required, Range(0, 10)]
    public int MaxTabSwitches { get; set; } = 3;

    [Required]
    public DateTime StartDate { get; set; } = DateTime.UtcNow;

    [Required]
    public DateTime EndDate { get; set; } = DateTime.UtcNow.AddDays(1);

    public bool ShowResult { get; set; } = false;
    public List<Guid> SelectedQuestionIds { get; set; } = new();
}