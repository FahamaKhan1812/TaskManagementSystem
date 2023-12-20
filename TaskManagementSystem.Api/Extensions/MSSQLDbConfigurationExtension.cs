using Microsoft.EntityFrameworkCore;
using TaskManagementSystem.DAL.Data;

namespace TaskManagementSystem.Api.Extensions;
public static class MSSQLDbConfigurationExtension
{
    public static void AddMSDb(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<DataContext>(opt =>
        {
            opt.UseSqlServer(configuration.GetConnectionString("MSSQLConnector"));
        });
    }
}
