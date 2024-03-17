using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TaskManagementSystem.DAL.Data;
using TaskManagementSystem.Domain.IRepositories;

namespace TaskManagementSystem.DAL.Repositories;

/// <summary>
/// Generic repository implementation for database operations.
/// </summary>
/// <typeparam name="TEntity">The type of entity managed by this repository.</typeparam>
public class Repository<TEntity> : IRepository<TEntity> where TEntity : class, new()
{
    internal readonly DataContext _dbContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="Repository{TEntity}"/> class.
    /// </summary>
    /// <param name="dbContext">The data context used for database operations.</param>
    public Repository(DataContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <summary>
    /// Asynchronously adds an entity to the database.
    /// </summary>
    /// <param name="entity">The entity to add.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
    /// <returns>A task representing the asynchronous operation, returning the added entity.</returns>
    public async Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken)
    {
        _ = await _dbContext.Set<TEntity>().AddAsync(entity, cancellationToken);
        _ = await _dbContext.SaveChangesAsync(cancellationToken);
        return entity;
    }

    /// <summary>
    /// Asynchronously adds a list of entities to the database.
    /// </summary>
    /// <param name="entity">The list of entities to add.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
    public async Task AddAsync(List<TEntity> entity, CancellationToken cancellationToken)
    {
        await _dbContext.Set<TEntity>().AddRangeAsync(entity, cancellationToken);
        _ = await _dbContext.SaveChangesAsync(cancellationToken);
    }

    /// <summary>
    /// Asynchronously deletes an entity from the database.
    /// </summary>
    /// <param name="entity">The entity to delete.</param>
    /// <param name="cancellationToken">The cancellation token.</param
    public async Task DeleteAsync(TEntity entity, CancellationToken cancellationToken)
    {
        _ = _dbContext.Set<TEntity>().Remove(entity);
        _ = await _dbContext.SaveChangesAsync(cancellationToken);
    }

    /// <summary>
    /// Asynchronously deletes a list of entities from the database.
    /// </summary>
    /// <param name="entities">The list of entities to delete.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    public async Task DeleteAsync(List<TEntity> entities, CancellationToken cancellationToken)
    {
        _dbContext.Set<TEntity>().RemoveRange(entities);
        _ = await _dbContext.SaveChangesAsync(cancellationToken);
    }

    /// <summary>
    /// Asynchronously updates an entity in the database.
    /// </summary>
    /// <param name="entity">The entity to update.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    public async Task UpdateAsync(TEntity entity, CancellationToken cancellationToken)
    {
        _dbContext.Entry(entity).State = EntityState.Modified;
        _ = await _dbContext.SaveChangesAsync(cancellationToken);
    }

    /// <summary>
    /// Asynchronously retrieves all entities from the database based on the provided predicates.
    /// </summary>
    /// <param name="wheres">The predicates to filter entities.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A list of entities that match the predicates.</returns>
    public virtual async Task<IList<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>>[] wheres, CancellationToken cancellationToken)
    {
        IQueryable<TEntity> query = _dbContext.Set<TEntity>();
        foreach (var predicate in wheres)
        {
            query = query.Where(predicate);
        }
        return await query
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    /// <summary>
    /// Asynchronously retrieves all entities from the database.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A list of all entities.</returns>
    public async Task<IList<TEntity>> GetAllAsync(CancellationToken cancellationToken)
    {
        IQueryable<TEntity> query = _dbContext.Set<TEntity>();
        return await query
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    /// <summary>
    /// Asynchronously retrieves an entity from the database based on the provided predicates.
    /// </summary>
    /// <param name="wheres">The predicates to filter the entity.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The first entity that matches the predicates, or null if none found.</returns>
    public async Task<TEntity?> GetAsync(Expression<Func<TEntity, bool>>[] wheres, CancellationToken cancellationToken)
    {
        IQueryable<TEntity> query = _dbContext.Set<TEntity>();
        foreach (var predicate in wheres)
        {
            query = query.Where(predicate);
        }

        return await query
                .AsNoTracking()
                .FirstOrDefaultAsync(cancellationToken);
    }

}