using System.Linq.Expressions;

namespace TaskManagementSystem.Domain.IRepositories;

/// <summary>
/// Represents a generic repository interface for data access operations.
/// </summary>
/// <typeparam name="TEntity">The type of entity managed by the repository.</typeparam>
public interface IRepository<TEntity> where TEntity : class, new()
{
    /// <summary>
    /// Asynchronously adds a single entity to the repository.
    /// </summary>
    /// <param name="entity">The entity to add.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken);

    /// <summary>
    /// Asynchronously adds a list of entities to the repository.
    /// </summary>
    /// <param name="entities">The list of entities to add.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task AddAsync(List<TEntity> entities, CancellationToken cancellationToken);

    /// <summary>
    /// Asynchronously deletes a single entity from the repository.
    /// </summary>
    /// <param name="entity">The entity to delete.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task DeleteAsync(TEntity entity, CancellationToken cancellationToken);

    /// <summary>
    /// Asynchronously deletes a list of entities from the repository.
    /// </summary>
    /// <param name="entities">The list of entities to delete.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task DeleteAsync(List<TEntity> entities, CancellationToken cancellationToken);

    /// <summary>
    /// Asynchronously retrieves all entities from the repository that match the specified conditions.
    /// </summary>
    /// <param name="wheres">An array of predicate expressions to filter entities.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task representing the asynchronous operation, returning a list of entities.</returns>
    Task<IList<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>>[] wheres, CancellationToken cancellationToken);

    /// <summary>
    /// Asynchronously retrieves all entities from the repository.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task representing the asynchronous operation, returning a list of entities.</returns>
    Task<IList<TEntity>> GetAllAsync(CancellationToken cancellationToken);

    /// <summary>
    /// Asynchronously retrieves a single entity from the repository that matches the specified conditions.
    /// </summary>
    /// <param name="wheres">An array of predicate expressions to filter entities.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task representing the asynchronous operation, returning the entity if found; otherwise, null.</returns>
    Task<TEntity?> GetAsync(Expression<Func<TEntity, bool>>[] wheres, CancellationToken cancellationToken);

    /// <summary>
    /// Asynchronously updates an entity in the repository.
    /// </summary>
    /// <param name="entity">The entity to update.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task UpdateAsync(TEntity entity, CancellationToken cancellationToken);

}
