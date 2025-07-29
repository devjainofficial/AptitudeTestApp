using System.ComponentModel.DataAnnotations;

namespace AptitudeTestApp.Application.DTOs;

public class StudentTestStartDto : BaseDto<Guid>
{
    [Required(ErrorMessage = "Full Name is required")]
    public string StudentName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email address")]
    public string StudentEmail { get; set; } = string.Empty;

    public string Token { get; set; } = string.Empty;

}
