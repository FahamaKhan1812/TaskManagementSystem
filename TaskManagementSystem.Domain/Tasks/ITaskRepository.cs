using TaskManagementSystem.Domain.IRepositories;

namespace TaskManagementSystem.Domain.Tasks;
public interface ITaskRepository : IRepository<Task>
{
    public Task<IList<Task>> GetAllAsync(int page, int pageSize, CancellationToken cancellationToken);
    public Task<Task?> GetAsync(Guid taskId, CancellationToken cancellationToken);

}
