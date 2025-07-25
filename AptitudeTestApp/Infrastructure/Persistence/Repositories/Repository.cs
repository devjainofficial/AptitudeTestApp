namespace AptitudeTestApp.Infrastructure.Persistence.Repositories;

using System.Linq.Expressions;
using AptitudeTestApp.Application.Interfaces;
using Microsoft.EntityFrameworkCore;
public class Repository<TContext>(TContext context) : IRepository
        where TContext : DbContext
{
    public async Task<TEntity?> GetByIdAsync<TEntity>(Guid id,
        CancellationToken cancellationToken = default)
        where TEntity : class => await context.Set<TEntity>().FindAsync([id],
            cancellationToken: cancellationToken);

    public async Task<IEnumerable<TEntity>> GetAllAsync<TEntity>(CancellationToken cancellationToken = default)
        where TEntity : class => await context.Set<TEntity>().ToListAsync(cancellationToken);

    public async Task<IEnumerable<TEntity>> GetAllAsync<TEntity>(
        Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken = default)
        where TEntity : class => await context.Set<TEntity>().Where(predicate).AsNoTracking().ToListAsync(cancellationToken);

    public IQueryable<TEntity> GetQueryable<TEntity>()
        where TEntity : class => context.Set<TEntity>().AsQueryable();

    public async Task AddAsync<TEntity>(TEntity entity,
        bool saveChanges = true,
        CancellationToken cancellationToken = default)
        where TEntity : class
    {
        await context.Set<TEntity>().AddAsync(entity, cancellationToken);

        if (saveChanges)
            await context.SaveChangesAsync(cancellationToken);
    }

    public async Task AddRangeAsync<TEntity>(IEnumerable<TEntity> entities,
        bool saveChanges = true,
        CancellationToken cancellationToken = default)
        where TEntity : class
    {
        await context.Set<TEntity>().AddRangeAsync(entities, cancellationToken);

        if (saveChanges)
            await context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateRangeAsync<TEntity>(IEnumerable<TEntity> entities,
        bool saveChanges = true,
        CancellationToken cancellationToken = default)
        where TEntity : class
    {
        context.Set<TEntity>().UpdateRange(entities);

        if (saveChanges)
            await context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync<TEntity>(TEntity entity,
        bool saveChanges = true,
        CancellationToken cancellationToken = default)
        where TEntity : class
    {
        context.Set<TEntity>().Update(entity);

        if (saveChanges)
            await context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync<TEntity>(Guid id,
        bool saveChanges = true,
        CancellationToken cancellationToken = default)
        where TEntity : class
    {
        TEntity? entity = await context.Set<TEntity>().FindAsync(id);
        if (entity != null)
        {
            context.Set<TEntity>().Remove(entity);

            if (saveChanges)
                await context.SaveChangesAsync(cancellationToken);
        }
    }
    public async Task RemoveAsync<TEntity>(TEntity entity,
        bool saveChanges = true,
        CancellationToken cancellationToken = default)
        where TEntity : class
    {
        context.Set<TEntity>().Remove(entity);

        if (saveChanges)
            await context.SaveChangesAsync(cancellationToken);
    }

    public async Task RemoveRangeAsync<TEntity>(IEnumerable<TEntity> entities,
        bool saveChanges = true,
        CancellationToken cancellationToken = default)
        where TEntity : class
    {
        context.Set<TEntity>().RemoveRange(entities);

        if (saveChanges)
            await context.SaveChangesAsync(cancellationToken);
    }

    public IQueryable<T> GetSqlQuery<T>(string sql,
        params object[] parameters) => context.Database.SqlQueryRaw<T>(sql, parameters).AsQueryable();

    public async Task<TResult> ExecuteInTransactionAsync<TResult>(Func<Task<TResult>> action,
        CancellationToken cancellationToken = default)
    {
        var strategy = context.Database.CreateExecutionStrategy();
        return await strategy.ExecuteAsync(async () =>
        {
            // Start the transaction
            await using var transaction = await context.Database.BeginTransactionAsync(cancellationToken);

            try
            {
                // Execute the provided action and get the result
                var result = await action();

                // Commit the transaction
                await transaction.CommitAsync(cancellationToken);

                return result;
            }
            catch
            {
                // Rollback the transaction in case of an error
                await transaction.RollbackAsync(cancellationToken);
                throw;
            }
        });
    }

    public async Task ExecuteInTransactionAsync(Func<Task> action,
        CancellationToken cancellationToken = default)
    {
        var strategy = context.Database.CreateExecutionStrategy();
        await strategy.ExecuteAsync(async () =>
        {
            // Start the transaction
            await using var transaction = await context.Database.BeginTransactionAsync(cancellationToken);

            try
            {
                // Execute the provided action
                await action();

                // Commit the transaction
                await transaction.CommitAsync(cancellationToken);
            }
            catch
            {
                // Rollback the transaction in case of an error
                await transaction.RollbackAsync(cancellationToken);
                throw;
            }
        });
    }

    public async Task SaveChangesAsync(CancellationToken ctx = default) =>
        await context.SaveChangesAsync(ctx);
}

