using Hangfire;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using TaskManagementSystem.Application.Options;
using TaskManagementSystem.Application.Services;
using TaskManagementSystem.DAL.Repositories;
using TaskManagementSystem.Domain.Categories;
using TaskManagementSystem.Domain.IRepositories;
using TaskManagementSystem.Domain.Tasks;
using TaskManagementSystem.Domain.Users;

namespace TaskManagementSystem.Application.Extensions;
public static class ApplicationServiceExtension
{
    public static void AddApplicationLayer(this IServiceCollection services, IConfiguration configuration)
    {
        //registration of MediatR
        services.AddMediatR(conf => conf.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

        //registration of AutoMapper    
        services.AddAutoMapper(Assembly.GetExecutingAssembly());

        // Hangfire setup
        services.AddHangfire(opt => opt
            .UseSqlServerStorage(configuration.GetConnectionString("MSSQLConnector")));
        //
        services.AddScoped<JwtSetting>();
        services.AddScoped<IdentityServices>();
        services.AddScoped<IEmailService, EmailService>();

        // Register the generic repository
        _ = services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

        // Register the category repository
        _ = services.AddScoped<ICategoryRepository, CategoryRepository>();

        // Register the task repository
        _ = services.AddScoped<ITaskRepository, TaskRepository>();

        // Register the task repository
        _ = services.AddScoped<IUserRepository, UserRepository>();


    }
}
