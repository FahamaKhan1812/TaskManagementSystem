using Microsoft.AspNetCore.Mvc.ApiExplorer;

namespace TaskManagementSystem.Api.Extensions;
internal static class SwaggerConfigurationExtension
{
    public static void UseSwaggerWithUI(this IApplicationBuilder app)
    {
        var provider = app.ApplicationServices.GetRequiredService<IApiVersionDescriptionProvider>();
        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            foreach (var description in provider.ApiVersionDescriptions)
            {
                options.SwaggerEndpoint(
                    $"/swagger/{description.GroupName}/swagger.json",
                    description.GroupName.ToUpperInvariant());
            }
        });
    }
}
