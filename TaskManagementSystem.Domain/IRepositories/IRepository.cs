using System.Linq.Expressions;

namespace TaskManagementSystem.Domain.IRepositories;

public interface IRepository<TEntity> where TEntity : class, new()
{
    Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken);
    
    Task AddAsync(List<TEntity> entity, CancellationToken cancellationToken);
    
    Task DeleteAsync(TEntity entity, CancellationToken cancellationToken);
    
    Task DeleteAsync(List<TEntity> entities, CancellationToken cancellationToken);
    
    Task<IList<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>>[] wheres, CancellationToken cancellationToken);
    
    Task<IList<TEntity>> GetAllAsync(CancellationToken cancellationToken);
    
    Task<TEntity?> GetAsync(Expression<Func<TEntity, bool>>[] wheres, CancellationToken cancellationToken);
    
    Task UpdateAsync(TEntity entity, CancellationToken cancellationToken);
}