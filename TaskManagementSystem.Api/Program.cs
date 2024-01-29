using TaskManagementSystem.Application.Extensions;
using TaskManagementSystem.Api.Extensions;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using TaskManagementSystem.Api.Options;
using NLog.Web;
using NLog;
using System.Text.Json;
using TaskManagementSystem.Api.Middlewares;
using Hangfire;
using TaskManagementSystem.Application.Notifications;

var logger = LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
try
{
    var builder = WebApplication.CreateBuilder(args);

    // Add services to the container.

    builder.Services.AddControllers();
    builder.Services.AddDependencyInjectionServices();
    builder.Services.AddApiVersion();
    builder.Services.AddMSDb(builder.Configuration);
    builder.Services.AddIdentityDb();
    builder.Services.AddJwtAuthentication(builder.Configuration);
    builder.Services.AddApplication(builder.Configuration);

    
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
    builder.Services.ConfigureOptions<ConfigureSwaggerOptions>();

    builder.Logging.ClearProviders();
    builder.Host.UseNLog();


    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI(opt =>
        {
            IApiVersionDescriptionProvider provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
        foreach(ApiVersionDescription desc in provider.ApiVersionDescriptions)
            {
                opt.SwaggerEndpoint(
                   $"/swagger/{desc.GroupName}/swagger.json", desc.ApiVersion.ToString()
                   );
            }
        });
    }

    app.UseHttpsRedirection();

    app.UseAuthentication();
    
    app.UseStatusCodePages(async context =>
    {
        if (context.HttpContext.Response.StatusCode == 401)
        {
            context.HttpContext.Response.ContentType = "application/json";

            var errorResponse = new
            {
                StatusCode = 401,
                Message = "Unauthorized. Please provide a valid JWT."
            };

            var jsonResponse = JsonSerializer.Serialize(errorResponse);
            await context.HttpContext.Response.WriteAsync(jsonResponse);
        }
    });


    app.UseAuthorization();
    
    //app.UseMiddleware<ExceptionHandlingMiddleware>();


    // Configure Hangfire dashboard
    app.UseHangfireDashboard();

    // Configure Hangfire server
    app.UseHangfireServer();

    // Schedule the EmailNotificationJob to run
    RecurringJob.AddOrUpdate<EmailNotificationJob>("SendTaskReminders", job => job.Execute(), "0 */15 * * * ?");

    // Every 15 mins => 0 */ 15 * ** ?
    // Every 2 secs => 0/2 * * ? * *

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
    NLog.LogManager.Shutdown();
}