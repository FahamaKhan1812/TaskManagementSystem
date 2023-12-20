using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using TaskManagementSystem.Application.Options;
using TaskManagementSystem.Application.Services;

namespace TaskManagementSystem.Application.Extensions;
public static class ApplicationServiceExtension
{
    public static void AddApplication(this IServiceCollection services)
    {
        //registration of MediatR
        services.AddMediatR(conf => conf.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

        //registration of AutoMapper    
        services.AddAutoMapper(Assembly.GetExecutingAssembly());

        //
        services.AddScoped<JwtSetting>();
        services.AddScoped<IdentityServices>();

    }
}
