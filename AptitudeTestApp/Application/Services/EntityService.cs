using AptitudeTestApp.Application.DTOs;
using AptitudeTestApp.Application.Interfaces;
using AptitudeTestApp.Data.Models;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace AptitudeTestApp.Application.Services;

public abstract class EntityService<TDto, TEntity>(IRepository repository) : IEntityService<TDto, Guid>
    where TEntity : BaseEntity<Guid>
    where TDto : BaseDto<Guid>
{
    protected readonly IRepository Repo = repository;

    public virtual async Task<List<TDto>> GetAllAsync(Guid creatorId, int skip, int take, CancellationToken cancellationToken = default)
    {
        var query = Repo.GetQueryable<TEntity>()
            .Where(e => e.CreatorId == creatorId);

        List<TEntity>? entities = await query
            .Skip(skip)
            .Take(take)
            .ToListAsync(cancellationToken);

        return entities.Adapt<List<TDto>>();
    }

    public virtual async Task<TDto?> GetByIdAsync(Guid id, Guid creatorId, CancellationToken cancellationToken = default)
    {
        TEntity? entity = await Repo.GetQueryable<TEntity>()
            .FirstOrDefaultAsync(e => e.Id == id && e.CreatorId == creatorId, cancellationToken);

        return entity?.Adapt<TDto>();
    }


    public virtual async Task CreateAsync(TDto dto, Guid? creatorId, CancellationToken cancellationToken = default)
    {
        TEntity? entity = dto.Adapt<TEntity>();
        
        entity.CreatorId = creatorId;
        
        await Repo.AddAsync(entity, cancellationToken : cancellationToken);
    }

    public virtual async Task UpdateAsync(TDto dto, Guid creatorId, CancellationToken cancellationToken = default)
    {
        TEntity? existing = await Repo.GetQueryable<TEntity>()
            .FirstOrDefaultAsync(e => e.Id == dto.Id && e.CreatorId == creatorId, cancellationToken);

        if (existing == null)
            throw new UnauthorizedAccessException("Unauthorized or not found.");

        dto.Adapt(existing);

        existing.CreatorId ??= creatorId;

        await Repo.UpdateAsync(existing, cancellationToken: cancellationToken);
    }


    public virtual async Task DeleteAsync(Guid id, Guid creatorId, CancellationToken cancellationToken = default)
    {
        TEntity? existing = await Repo.GetQueryable<TEntity>()
            .FirstOrDefaultAsync(e => e.Id == id && e.CreatorId == creatorId, cancellationToken);

        if (existing == null)
            throw new UnauthorizedAccessException("Unauthorized or not found.");

        await Repo.DeleteAsync<TEntity>(id, cancellationToken: cancellationToken);
    }


    public virtual async Task<int> GetTotalCountAsync(Guid CreatorId, CancellationToken cancellationToken = default)
    {
        int count = await Repo.GetQueryable<TEntity>()
            .Where(e => e.CreatorId == CreatorId)
            .CountAsync(cancellationToken);

        return count;
    }

}
