using TaskManagementSystem.DAL.Repositories;
using TaskManagementSystem.Domain.IRepositories;

namespace TaskManagementSystem.Api.Extensions;

public static class DependencyInjectionExtension
{
    public static void AddDependencyInjectionServices(this IServiceCollection services)
    {
        // Register the generic repository
        _ = services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        
        // Register the category repository
        _ = services.AddScoped<ICategoryRepository, CategoryRepository>();
    }
}
