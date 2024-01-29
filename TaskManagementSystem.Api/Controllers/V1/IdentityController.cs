using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManagementSystem.Api.Extensions;
using TaskManagementSystem.Application.Contracts.Identity.Request;
using TaskManagementSystem.Application.Contracts.Identity.Response;
using TaskManagementSystem.Application.Identity.Commands;
using TaskManagementSystem.Application.Identity.Queries;

namespace TaskManagementSystem.Api.Controllers.V1;

[ApiVersion("1.0")]
[Route(ApiRoutes.BaseRoute)]
[ApiController]
public class IdentityController : BaseController
{
    private readonly IMediator _mediator;

    public IdentityController(IMediator mediator, ILogger<BaseController> logger) : base(logger)
    {
        _mediator = mediator;
    }

    [HttpPost(ApiRoutes.Identity.RegisterNewIdentity)]
    public async Task<IActionResult> Register([FromBody] RegistrationRequest registration)
    {
        var command = new RegisterCommand()
        {
            Email = registration.UserName,
            FirstName = registration.FirstName,
            LastName = registration.LastName,
            Password = registration.Password,
            Username = registration.UserName,
        };
        var result = await _mediator.Send(command);
        return result.IsError ? HandleErrorResponse(result.Errors) : Ok(result.Payload);
    }

    [HttpPost(ApiRoutes.Identity.LoginIdentity)]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var command = new LoginCommand()
        {
            UserName = request.UserName,
            Password = request.Password
        };
        var result = await _mediator.Send(command);
        var authResult = new AuthenticationResponse()
        {
            Token = result.Payload,
        };
        return result.IsError ? HandleErrorResponse(result.Errors) : Ok(authResult);
    }

    [HttpGet]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> GetAllIdentities()
    {
        var userClaim = HttpContext.GetUserRole();
        var query = new GetAllIdentity()
        {
            UserRole = userClaim
        };
        var result = await _mediator.Send(query);
        return result.IsError ? HandleErrorResponse(result.Errors) : Ok(result.Payload);
    }

    [HttpGet(ApiRoutes.Identity.IdRoute)]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> GetIdentityById(Guid identityId)
    {
        var userClaim = HttpContext.GetUserRole();
        var query = new GetIdentityById()
        {
            UserRole = userClaim,
            IdentityId = identityId
        };
        var result = await _mediator.Send(query);
        return result.IsError ? HandleErrorResponse(result.Errors) : Ok(result.Payload);
    }

    [HttpPatch(ApiRoutes.Identity.IdRoute)]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> UpdateIdentity(Guid identityId, [FromBody] UpdateIdentityRequest request)
    {
        var userClaim = HttpContext.GetUserRole();
        var command = new UpdateIdentityCommand()
        {
            UserId = identityId,
            FirstName = request.FirstName,
            LastName = request.LastName,
            UserName = request.UserName,
            UserRole = userClaim!
        };
        var result = await _mediator.Send(command);
        return result.IsError ? HandleErrorResponse(result.Errors) : NoContent();

    }

    [HttpDelete(ApiRoutes.Identity.IdRoute)]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> DeleteIdentity(Guid identityId)
    {
        var userClaim = HttpContext.GetUserRole();
        var command = new RemoveIdentityCommand()
        {
            UserId = identityId,
            UserRole = userClaim!
        };
        var result = await _mediator.Send(command);
        return result.IsError ? HandleErrorResponse(result.Errors) : NoContent();
    }
}
