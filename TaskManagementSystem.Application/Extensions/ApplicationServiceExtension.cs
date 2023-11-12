using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace TaskManagementSystem.Application.Extensions;
public static class ApplicationServiceExtension
{
    public static void AddApplication(this IServiceCollection services)
    {
        //registration of MediatR
        services.AddMediatR(conf => conf.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

        //registration of AutoMapper
        services.AddAutoMapper(Assembly.GetExecutingAssembly());

    }
}
