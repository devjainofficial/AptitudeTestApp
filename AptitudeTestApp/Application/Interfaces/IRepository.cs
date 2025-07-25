namespace AptitudeTestApp.Application.Interfaces;

using System.Linq.Expressions;

public interface IRepository
{
    Task<TEntity?> GetByIdAsync<TEntity>(Guid id,
        CancellationToken cancellationToken = default)
        where TEntity : class;

    IQueryable<TEntity> GetQueryable<TEntity>()
        where TEntity : class;

    Task<IEnumerable<TEntity>> GetAllAsync<TEntity>(CancellationToken cancellationToken = default)
        where TEntity : class;

    Task<IEnumerable<TEntity>> GetAllAsync<TEntity>(Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken = default)
        where TEntity : class;

    Task AddAsync<TEntity>(TEntity entity,
        bool saveChanges = true,
        CancellationToken cancellationToken = default)
        where TEntity : class;

    Task AddRangeAsync<TEntity>(IEnumerable<TEntity> entities,
        bool saveChanges = true,
        CancellationToken cancellationToken = default)
        where TEntity : class;

    Task UpdateAsync<TEntity>(TEntity entity,
        bool saveChanges = true,
        CancellationToken cancellationToken = default)
        where TEntity : class;

    Task DeleteAsync<TEntity>(Guid id,
        bool saveChanges = true,
        CancellationToken cancellationToken = default)
        where TEntity : class;

    Task RemoveAsync<TEntity>(TEntity entity,
        bool saveChanges = true,
        CancellationToken cancellationToken = default)
        where TEntity : class;

    Task RemoveRangeAsync<TEntity>(IEnumerable<TEntity> entities,
        bool saveChanges = true,
        CancellationToken cancellationToken = default)
        where TEntity : class;

    Task<TResult> ExecuteInTransactionAsync<TResult>(Func<Task<TResult>> action,
        CancellationToken cancellationToken = default);

    Task ExecuteInTransactionAsync(Func<Task> action,
        CancellationToken cancellationToken = default);

    IQueryable<T> GetSqlQuery<T>(string sql,
        params object[] parameters);

    Task SaveChangesAsync(CancellationToken ctx = default);

    Task UpdateRangeAsync<TEntity>(IEnumerable<TEntity> entities,
        bool saveChanges = true,
        CancellationToken cancellationToken = default)
        where TEntity : class;
}
