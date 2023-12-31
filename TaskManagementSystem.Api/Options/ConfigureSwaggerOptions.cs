using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace TaskManagementSystem.Api.Options;
public class ConfigureSwaggerOptions : IConfigureOptions<SwaggerGenOptions>
{
    private readonly IApiVersionDescriptionProvider _provider;
    public ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider)
    {
        _provider = provider;
    }
    public void Configure(SwaggerGenOptions options)
    {
        foreach (var desc in _provider.ApiVersionDescriptions)
        {
            options.SwaggerDoc(desc.GroupName, CreateVersionInfo(desc));
        }
    }
    private OpenApiInfo CreateVersionInfo(ApiVersionDescription apiVersion)
    {
        var info = new OpenApiInfo
        {
            Title = "Task Management System",
            Version = apiVersion.ApiVersion.ToString()
        };

        // Check for the deprecated version
        if(apiVersion.IsDeprecated)
        {
            info.Description = "This api has been deprecated";
        }

        return info;
    }
}
