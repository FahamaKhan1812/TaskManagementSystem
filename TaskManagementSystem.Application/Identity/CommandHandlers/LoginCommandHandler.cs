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
using TaskManagementSystem.Domain.Entities;

namespace TaskManagementSystem.Application.Identity.CommandHandlers;
internal class LoginCommandHandler : IRequestHandler<LoginCommand, OperationResult<string>>
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IdentityServices _identityServices;
    private readonly RoleManager<IdentityRole> _roleManager;

    public LoginCommandHandler(UserManager<IdentityUser> userManager, IdentityServices identityServices, RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _identityServices = identityServices;
        _roleManager = roleManager;
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
            var userRoles = await _userManager.GetRolesAsync(identityUser);
            if (userRoles.Count != 0)
            {
                result.Payload = GenerateJwt(identityUser, userRoles[0]);
            }
            else 
            { 
                result.Payload = GenerateJwt(identityUser);
            }
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

    private string GenerateJwt(IdentityUser user, string userRolesList="User")
    {
        var claimsIdentity = new ClaimsIdentity(new Claim[]
        {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTime.Now.ToString()),
                new Claim("UserId", user.Id),
                new Claim(ClaimTypes.Role, userRolesList)
         });
        var token = _identityServices.CreateSecurityToken(claimsIdentity);
        return _identityServices.WriteToken(token);
    }
}
