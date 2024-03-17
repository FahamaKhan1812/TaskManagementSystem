using NLog;
using System.Text.Json;

namespace TaskManagementSystem.Api.Extensions;
internal static class MvcWebAppRegisterExtension
{
    public static void RegisterPipelineComponents(this IApplicationBuilder app, Logger logger)
    {
        app.UseSwaggerWithUI();
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
                logger.Info(jsonResponse);
            }
        });

        app.UseAuthorization();

        //app.UseMiddleware<ExceptionHandlingMiddleware>();
        app.UseHangfire();

    }
}
