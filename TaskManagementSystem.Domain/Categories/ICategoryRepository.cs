using TaskManagementSystem.Domain.IRepositories;

namespace TaskManagementSystem.Domain.Categories;

public interface ICategoryRepository
    : IRepository<Category>
{
    public bool IsUserAdmin(string role);

}