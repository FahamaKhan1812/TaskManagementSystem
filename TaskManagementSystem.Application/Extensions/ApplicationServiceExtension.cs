using Hangfire;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using TaskManagementSystem.Application.Options;
using TaskManagementSystem.Application.Services;

namespace TaskManagementSystem.Application.Extensions;
public static class ApplicationServiceExtension
{
    public static void AddApplication(this IServiceCollection services, IConfiguration configuration)
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

    }
}
