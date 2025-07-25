using System.ComponentModel.DataAnnotations;

namespace AptitudeTestApp.Application.DTOs;

public class CreateTestSessionDto
{
    [Required, MaxLength(200)]
    public string TestName { get; set; } = string.Empty;

    public string? Description { get; set; }

    [Required]
    public Guid UniversityId { get; set; }

    [Range(10, 480)] // 10 minutes to 8 hours
    public int TimeLimit { get; set; } = 60;

    [Range(1, 100)]
    public int TotalQuestions { get; set; } = 20;

    [Range(0, 100)]
    public decimal PassingScore { get; set; } = 60.00m;

    [Range(0, 10)]
    public int MaxTabSwitches { get; set; } = 3;

    [Required]
    public DateTime StartDate { get; set; }

    [Required]
    public DateTime EndDate { get; set; }

    public List<Guid> SelectedQuestionIds { get; set; } = new();
}