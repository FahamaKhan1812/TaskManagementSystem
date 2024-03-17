using TaskManagementSystem.DAL.Data;
using TaskManagementSystem.Domain.Categories;
using TaskManagementSystem.Domain.Commons;

namespace TaskManagementSystem.DAL.Repositories;

public sealed class CategoryRepository
    : Repository<Category>, ICategoryRepository
{
    public CategoryRepository(DataContext dbContext) : base(dbContext)
    {
    }

    public bool IsUserAdmin(string role)
    {
        if (role == UserRole.Admin)
        {
            return true;
        }
        return false;
    }
}