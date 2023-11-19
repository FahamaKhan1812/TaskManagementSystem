using MediatR;
using TaskManagementSystem.Application.Contracts.Task.Response;
using TaskManagementSystem.Application.Models;

namespace TaskManagementSystem.Application.Tasks.Queries;
public class GetTaskById : IRequest<OperationResult<TaskWithCategoryDetailsResponse>>
{
    public Guid TaskId { get; set; }
}
