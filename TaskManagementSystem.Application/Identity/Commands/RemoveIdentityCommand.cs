using MediatR;
using TaskManagementSystem.Application.Models;

namespace TaskManagementSystem.Application.Identity.Commands;
public class RemoveIdentityCommand : IRequest<OperationResult<string>>
{
    public Guid UserId { get; set; }
    public string UserRole { get; set; }
}
