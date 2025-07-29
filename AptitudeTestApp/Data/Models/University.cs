using System.ComponentModel.DataAnnotations;

namespace AptitudeTestApp.Data.Models;
public class University : BaseEntity<Guid>
{
    [Required, MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    [Required, MaxLength(50)]
    public string Code { get; set; } = string.Empty;

    [MaxLength(255)]
    public string? ContactEmail { get; set; }

    public bool IsActive { get; set; } = true;
    public string? CreatedBy { get; set; }

    // Navigation Properties
    public virtual ICollection<TestSession> TestSessions { get; set; } = new List<TestSession>();
}