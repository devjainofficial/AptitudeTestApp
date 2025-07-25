namespace AptitudeTestApp.Data.Models; 
public class TestSessionQuestion : BaseEntity<Guid>
{
    public Guid TestSessionId { get; set; }
    public Guid QuestionId { get; set; }
    public int DisplayOrder { get; set; } = 0;

    // Navigation Properties
    public virtual TestSession TestSession { get; set; } = null!;
    public virtual Question Question { get; set; } = null!;
}
