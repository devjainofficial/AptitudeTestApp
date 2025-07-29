using System.ComponentModel.DataAnnotations;

namespace AptitudeTestApp.Application.DTOs;

public class QuestionCategoryDto : BaseDto<Guid>
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool IsActive { get; set; } = true;
}
