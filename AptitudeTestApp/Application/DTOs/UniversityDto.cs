namespace AptitudeTestApp.Application.DTOs;

public class UniversityDto : BaseDto<Guid>
{
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public string? ContactEmail { get; set; }
    public bool IsActive { get; set; }
}
