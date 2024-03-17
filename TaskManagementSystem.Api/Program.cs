using NLog;
using NLog.Web;
using TaskManagementSystem.Api.Extensions;
using TaskManagementSystem.Api.Options;
using TaskManagementSystem.Application.Extensions;

var logger = LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
try
{
    var builder = WebApplication.CreateBuilder(args);

    // Add services to the container.

    builder.Services.AddControllers();
    builder.Services.AddApiVersion();
    builder.Services.AddMSDb(builder.Configuration);
    builder.Services.AddIdentityDb();
    builder.Services.AddJwtAuthentication(builder.Configuration);
    builder.Services.AddApplicationLayer(builder.Configuration);
    builder.Services.AddHangfire();

    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
    builder.Services.ConfigureOptions<ConfigureSwaggerOptions>();

    builder.Logging.ClearProviders();
    builder.Host.UseNLog();

    var app = builder.Build();
    app.RegisterPipelineComponents(logger);
    app.MapControllers();

    app.Run();

}
catch (Exception ex)
{
    logger.Error(ex);
    throw;
}
finally
{
    LogManager.Shutdown();
}