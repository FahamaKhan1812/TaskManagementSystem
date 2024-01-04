using MediatR;
using TaskManagementSystem.Application.Contracts.Task.Response;
using TaskManagementSystem.Application.Models;
using TaskManagementSystem.Application.Tasks.QueryHandlers;

namespace TaskManagementSystem.Application.Tasks.Queries;
public class GetAllTasks : IRequest<OperationResult<TaskResponse>>
{
    public int Page { get; set; }
    public int PageSize { get; set; }
    public GetAllTasks(int page, int pageSize)
    {
        Page = page;
        PageSize = pageSize;
    }
}
