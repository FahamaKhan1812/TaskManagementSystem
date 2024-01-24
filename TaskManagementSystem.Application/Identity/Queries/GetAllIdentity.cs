using MediatR;
using TaskManagementSystem.Application.Contracts.Identity.Response;
using TaskManagementSystem.Application.Models;

namespace TaskManagementSystem.Application.Identity.Queries;
public class GetAllIdentity : IRequest<OperationResult<List<IdentityResponse>>>
{
    public string UserRole { get; set; }
}
