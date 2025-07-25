namespace AptitudeTestApp.Data.Models;

public class BaseEntity<T> where T : struct
{
    public T Id { get; set; } = default!;
}
