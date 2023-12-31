using MediatR;
using TaskManagementSystem.Application.Models;

namespace TaskManagementSystem.Application.Tasks.Commands;
public class DeleteTaskCommand : IRequest<OperationResult<string>>
{
    public Guid TaskId { get; set; }
    public Guid UserId { get; set; }
}
