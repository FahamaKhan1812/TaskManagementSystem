using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using TaskManagementSystem.Application.Options;

namespace TaskManagementSystem.Api.Extensions;
public static class JwtAuthenticationExtension
{
    public static void AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        // Configure JwtSetting from appsettings.json
        services.Configure<JwtSetting>(configuration.GetSection(nameof(JwtSetting)));

        // Configure and add authentication
        services.AddAuthentication(opt =>
        {
            opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            opt.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(jwt =>
        {
            var jwtSettings = configuration.GetSection(nameof(JwtSetting)).Get<JwtSetting>();
            var key = Encoding.ASCII.GetBytes(jwtSettings.Secret);

            jwt.SaveToken = true;

            jwt.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtSettings.Secret)),
                ValidateIssuer = true,
                ValidIssuer = jwtSettings.Issuer,
                ValidateAudience = true,
                ValidAudiences = jwtSettings.Audiences,
                RequireExpirationTime = false,
                ValidateLifetime = true,
            };
            jwt.Audience = jwtSettings.Audiences[0];
            jwt.ClaimsIssuer = jwtSettings.Issuer;
        });
    }
}
