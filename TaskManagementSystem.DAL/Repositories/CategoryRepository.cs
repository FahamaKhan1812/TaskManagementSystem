using TaskManagementSystem.DAL.Data;
using TaskManagementSystem.Domain.Entities;
using TaskManagementSystem.Domain.IRepositories;

namespace TaskManagementSystem.DAL.Repositories;

public class CategoryRepository
    : Repository<Category> , ICategoryRepository
{
    public CategoryRepository(DataContext dbContext) : base(dbContext)
    {
    }
}