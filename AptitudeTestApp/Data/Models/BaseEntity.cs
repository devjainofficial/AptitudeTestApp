
namespace AptitudeTestApp.Data.Models;

public class BaseEntity<T> where T : struct
{
    public T Id { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public Guid? CreatorId { get; set; }
}
