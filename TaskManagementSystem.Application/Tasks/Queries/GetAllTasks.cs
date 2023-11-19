using MediatR;
using TaskManagementSystem.Application.Contracts.Task.Response;
using TaskManagementSystem.Application.Models;

namespace TaskManagementSystem.Application.Tasks.Queries;
public class GetAllTasks : IRequest<OperationResult<List<TaskWithCategoryDetailsResponse>>>
{
}
