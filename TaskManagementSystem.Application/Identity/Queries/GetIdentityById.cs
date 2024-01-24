using MediatR;
using TaskManagementSystem.Application.Contracts.Identity.Response;
using TaskManagementSystem.Application.Models;

namespace TaskManagementSystem.Application.Identity.Queries;
public class GetIdentityById : IRequest<OperationResult<IdentityResponse>>
{
    public string UserRole { get; set; }
    public Guid IdentityId { get; set; }
}
