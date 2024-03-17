using AutoMapper;
using MediatR;
using TaskManagementSystem.Application.Contracts.Task.Response;
using TaskManagementSystem.Application.Enums;
using TaskManagementSystem.Application.Models;
using TaskManagementSystem.Application.Tasks.Queries;
using TaskManagementSystem.Domain.Tasks;

namespace TaskManagementSystem.Application.Tasks.QueryHandlers;
public class GetAllTasksHandler : IRequestHandler<GetAllTasks, OperationResult<TaskResponse>>
{
    private readonly IMapper _mapper;
    private readonly ITaskRepository _taskRepository;

    public GetAllTasksHandler(IMapper mapper, ITaskRepository taskRepository)
    {
        _mapper = mapper;
        _taskRepository = taskRepository;
    }

    public async Task<OperationResult<TaskResponse>> Handle(GetAllTasks request, CancellationToken cancellationToken)
    {
        var result = new OperationResult<TaskResponse>();

        try
        {
            var tasks = await _taskRepository.GetAllAsync(request.Page, request.PageSize, cancellationToken);

            var pageCount = Math.Ceiling(tasks.Count / (float)request.PageSize);

            var mappedData = _mapper.Map<List<TaskWithCategoryDetailsResponse>>(tasks);
            TaskResponse obj = new()
            {
                Pages = (int)pageCount,
                CurrentPage = request.Page,
                Tasks = mappedData
            };
            result.Payload = obj;
        }
        catch (Exception ex)
        {
            result.AddError(ErrorCode.UnknownError, ex.Message);
        }
        return result;

    }
}
