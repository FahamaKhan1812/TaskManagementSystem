using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using TaskManagementSystem.DAL.Data;
using TaskManagementSystem.Domain.IRepositories;

namespace TaskManagementSystem.DAL.Repositories;

public class Repository<TEntity> : IRepository<TEntity> where TEntity : class, new()
{
    internal readonly DataContext _dbContext;

    public Repository(DataContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken)
    {
        _ = await _dbContext.Set<TEntity>().AddAsync(entity, cancellationToken);
        _ = await _dbContext.SaveChangesAsync(cancellationToken);
        return entity;
    }

    public async Task AddAsync(List<TEntity> entity, CancellationToken cancellationToken)
    {
        await _dbContext.Set<TEntity>().AddRangeAsync(entity, cancellationToken);
        _ = await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(TEntity entity, CancellationToken cancellationToken)
    {
        _ = _dbContext.Set<TEntity>().Remove(entity);
        _ = await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(List<TEntity> entities, CancellationToken cancellationToken)
    {
        _dbContext.Set<TEntity>().RemoveRange(entities);
        _ = await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(TEntity entity, CancellationToken cancellationToken)
    {
        _dbContext.Entry(entity).State = EntityState.Modified;
        _ = await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public virtual async Task<IList<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>>[] wheres, CancellationToken cancellationToken)
    {
        IQueryable<TEntity> query = _dbContext.Set<TEntity>();
        foreach (var predicate in wheres)
        {
            query = query.Where(predicate);
        }

        return await query.ToListAsync(cancellationToken);
    }

    public async Task<IList<TEntity>> GetAllAsync(CancellationToken cancellationToken)
    {
        IQueryable<TEntity> query = _dbContext.Set<TEntity>();
        return await query.ToListAsync(cancellationToken);
    }

    public async Task<TEntity?> GetAsync(Expression<Func<TEntity, bool>>[] wheres, CancellationToken cancellationToken)
    {
        IQueryable<TEntity> query = _dbContext.Set<TEntity>();
        foreach (var predicate in wheres)
        {
            query = query.Where(predicate);
        }

        return await query.FirstOrDefaultAsync(cancellationToken);
    }
}