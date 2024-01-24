using MediatR;
using TaskManagementSystem.Application.Models;

namespace TaskManagementSystem.Application.Identity.Commands;
public class UpdateIdentityCommand : IRequest<OperationResult<string>>
{
    public Guid UserId { get; set; }
    public string UserName { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string UserRole { get; set; }
    public DateTime UpdatedAt { get; private set; } = DateTime.UtcNow;
}
