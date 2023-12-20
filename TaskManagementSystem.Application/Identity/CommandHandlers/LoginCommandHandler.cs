using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TaskManagementSystem.Application.Enums;
using TaskManagementSystem.Application.Identity.Commands;
using TaskManagementSystem.Application.Models;
using TaskManagementSystem.Application.Options;
using TaskManagementSystem.Application.Services;
using TaskManagementSystem.DAL.Data;

namespace TaskManagementSystem.Application.Identity.CommandHandlers;
internal class LoginCommandHandler : IRequestHandler<LoginCommand, OperationResult<string>>
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IdentityServices _identityServices;

    public LoginCommandHandler(UserManager<IdentityUser> userManager, IdentityServices identityServices)
    {
        _userManager = userManager;
        _identityServices = identityServices;
    }

    public async Task<OperationResult<string>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var result = new OperationResult<string>();

        try
        {
            var identityUser = await _userManager.FindByEmailAsync(request.UserName);
            if(identityUser == null) 
            {
                result.IsError = true;
                Error error = new()
                {
                    Code = ErrorCode.IdentityUserNotFound,
                    Message = "Invalid Credentials."
                };
                result.Errors.Add(error);
                return result;
            }

            var validPassword = await _userManager.CheckPasswordAsync(identityUser, request.Password);
            if (!validPassword)
            {
                result.IsError = true;
                Error error = new()
                {
                    Code = ErrorCode.IncorrectPassword,
                    Message = "Invalid Credentials."
                };
                result.Errors.Add(error);
                return result;
            }
            result.Payload = GenerateJwt(identityUser);
        }
        catch (Exception ex)
        {
            result.IsError = true;
            Error errors = new()
            {
                Code = ErrorCode.UnknownError,
                Message = ex.Message
            };
            result.Errors.Add(errors);
        }

        return result;
    }

    private string GenerateJwt(IdentityUser user)
    {
        var claimsIdentity = new ClaimsIdentity(new Claim[]
        {
              new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTime.Now.ToString()),
                new Claim("User Id", user.Id),
         });
        var token = _identityServices.CreateSecurityToken(claimsIdentity);
        return _identityServices.WriteToken(token);
    }
}
