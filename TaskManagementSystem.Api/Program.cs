using TaskManagementSystem.Application.Extensions;
using TaskManagementSystem.Api.Extensions;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using TaskManagementSystem.Api.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddApiVersion();
builder.Services.AddMSDb(builder.Configuration);
builder.Services.AddIdentityDb();
builder.Services.AddJwtAuthentication(builder.Configuration);
builder.Services.AddApplication();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.ConfigureOptions<ConfigureSwaggerOptions>();

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
app.UseAuthorization();

app.MapControllers();

app.Run();
