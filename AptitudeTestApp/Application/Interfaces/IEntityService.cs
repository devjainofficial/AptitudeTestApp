namespace AptitudeTestApp.Application.Interfaces;

public interface IEntityService<TDto, TId>
{
    Task<List<TDto>> GetAllAsync(Guid CreatorId, int skip, int take, CancellationToken cancellationToken = default);
    Task<TDto?> GetByIdAsync(TId id, Guid creatorId, CancellationToken cancellationToken = default);
    Task CreateAsync(TDto dto, Guid? creatorId, CancellationToken cancellationToken = default);
    Task UpdateAsync(TDto dto, Guid creatorId, CancellationToken cancellationToken = default);
    Task DeleteAsync(TId id, Guid creatorId, CancellationToken cancellationToken = default);
    Task<int> GetTotalCountAsync(Guid CreatorId, CancellationToken cancellationToken = default);
}
