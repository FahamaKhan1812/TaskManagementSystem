using TaskManagementSystem.DAL.Data;
using TaskManagementSystem.Domain.Users;

namespace TaskManagementSystem.DAL.Repositories;
public sealed class UserRepository
    : Repository<User>, IUserRepository
{
    public UserRepository(DataContext dbContext) : base(dbContext)
    {
    }
}
