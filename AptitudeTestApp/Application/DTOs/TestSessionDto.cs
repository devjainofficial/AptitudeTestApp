using System.ComponentModel.DataAnnotations;

namespace AptitudeTestApp.Application.DTOs;

public class TestSessionDto
{
    public Guid Id { get; set; }
    public Guid UniversityId { get; set; }
    public string TestName { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int TimeLimit { get; set; } // in minutes
    public int TotalQuestions { get; set; }
    public decimal PassingScore { get; set; } = 60.00m;
    public int MaxTabSwitches { get; set; } = 3;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string Token { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
}
