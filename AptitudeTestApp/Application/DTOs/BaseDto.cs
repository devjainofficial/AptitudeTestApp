namespace AptitudeTestApp.Application.DTOs;

public class BaseDto<T> where T : struct
{
    public T Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public Guid? CreatorId { get; set; }
}
