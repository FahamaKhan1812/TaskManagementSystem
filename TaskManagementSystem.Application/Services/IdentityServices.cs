using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

using TaskManagementSystem.Application.Options;

namespace TaskManagementSystem.Application.Services;
internal class IdentityServices
{
    private readonly JwtSetting _jwtSetting;
    private readonly byte[] _key;
    public IdentityServices(IOptions<JwtSetting> jwtSetting)
    {
        _jwtSetting = jwtSetting.Value;
        _key = Encoding.ASCII.GetBytes(_jwtSetting.Secret);
    }

    public JwtSecurityTokenHandler TokenHandler = new();
    public SecurityToken CreateSecurityToken(ClaimsIdentity claimsIdentity)
    {
        var tokenDescriptor = GetTokenDescriptor(claimsIdentity);
        return TokenHandler.CreateToken(tokenDescriptor);
    }
    public string WriteToken(SecurityToken token)
    {
        return TokenHandler.WriteToken(token);
    }
    private SecurityTokenDescriptor GetTokenDescriptor(ClaimsIdentity claimsIdentity)
    {
        return new SecurityTokenDescriptor()
        {
            Subject = claimsIdentity,
            Expires = DateTime.Now.AddHours(2),
            Audience = _jwtSetting.Audiences[0],
            Issuer = _jwtSetting.Issuer,
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(_key), SecurityAlgorithms.HmacSha256),
        };

    }
}
