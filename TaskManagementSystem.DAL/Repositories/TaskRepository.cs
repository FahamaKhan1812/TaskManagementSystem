using Microsoft.EntityFrameworkCore;
using TaskManagementSystem.DAL.Data;
using TaskManagementSystem.Domain.Tasks;

namespace TaskManagementSystem.DAL.Repositories;
public sealed class TaskRepository : Repository<Domain.Tasks.Task>, ITaskRepository
{
    public TaskRepository(DataContext dbContext) : base(dbContext)
    {
    }
    public async Task<IList<Domain.Tasks.Task>> GetAllAsync(int page, int pageSize, CancellationToken cancellationToken)
    {
        var query = _dbContext.Tasks.AsQueryable();
        var tasks = await query
            .OrderBy(t => t.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Include(t => t.Category)
            .Include(u => u.User)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        return tasks;

    }

    public async Task<Domain.Tasks.Task?> GetAsync(Guid taskId, CancellationToken cancellationToken)
    {
        var task = await _dbContext.Tasks
            .Include(t => t.Category)
            .Include(u => u.User)
            .AsNoTracking()
            .FirstOrDefaultAsync(t => t.Id == taskId, cancellationToken);

        return task;

    }
}