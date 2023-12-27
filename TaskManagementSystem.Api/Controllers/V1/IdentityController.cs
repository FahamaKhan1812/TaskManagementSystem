using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaskManagementSystem.Application.Contracts.Identity.Request;
using TaskManagementSystem.Application.Contracts.Identity.Response;
using TaskManagementSystem.Application.Identity.Commands;

namespace TaskManagementSystem.Api.Controllers.V1;

[Route("api/[controller]")]
[ApiController]
public class IdentityController : BaseController
{
    private readonly IMediator _mediator;

    public IdentityController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("register")]
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


    [HttpPost("login")]
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
}
