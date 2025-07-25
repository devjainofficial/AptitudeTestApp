namespace AptitudeTestApp.Application.DTOs;

public class QuestionDto
{
    public Guid Id { get; set; }
    public string QuestionText { get; set; } = string.Empty;
    public List<QuestionOptionDto> Options { get; set; } = new();
    public int Points { get; set; }
    public string CategoryName { get; set; } = string.Empty;
}

public class QuestionOptionDto
{
    public Guid Id { get; set; }
    public string OptionText { get; set; } = string.Empty;
}