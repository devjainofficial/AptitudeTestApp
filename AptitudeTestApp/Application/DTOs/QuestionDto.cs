using AptitudeTestApp.Shared.Enums;

namespace AptitudeTestApp.Application.DTOs;

public class QuestionDto : BaseDto<Guid>
{
    public string QuestionText { get; set; } = string.Empty;
    public List<QuestionOptionDto> Options { get; set; } = new();
    public int Points { get; set; }
    public string CategoryName { get; set; } = string.Empty;
    public Guid CategoryId { get; set; }
    public string QuestionType { get; set; } = "MultipleChoice";
    public QuestionDifficulty DifficultyLevel { get; set; } = QuestionDifficulty.Easy;
    public bool IsActive { get; set; } = true;
}

public class QuestionOptionDto : BaseDto<Guid>
{
    public string OptionText { get; set; } = string.Empty;
    public bool IsCorrect { get; set; }
}