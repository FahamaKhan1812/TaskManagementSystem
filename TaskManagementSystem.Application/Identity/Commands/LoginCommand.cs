using MediatR;
using TaskManagementSystem.Application.Contracts.Identity.Response;
using TaskManagementSystem.Application.Models;

namespace TaskManagementSystem.Application.Identity.Commands;
public class LoginCommand : IRequest<OperationResult<string>>
{
    public string UserName { get; set; }
    public string Password { get; set; }
}
