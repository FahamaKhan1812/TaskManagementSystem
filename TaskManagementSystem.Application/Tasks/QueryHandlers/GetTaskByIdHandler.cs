using AutoMapper;
using MediatR;
using TaskManagementSystem.Application.Contracts.Task.Response;
using TaskManagementSystem.Application.Enums;
using TaskManagementSystem.Application.Models;
using TaskManagementSystem.Application.Tasks.Queries;
using TaskManagementSystem.Domain.Tasks;

namespace TaskManagementSystem.Application.Tasks.QueryHandlers;
internal class GetTaskByIdHandler : IRequestHandler<GetTaskById, OperationResult<TaskWithCategoryDetailsResponse>>
{
    private readonly IMapper _mapper;
    private readonly ITaskRepository _taskRepository;

    public GetTaskByIdHandler(IMapper mapper, ITaskRepository taskRepository)
    {
        _mapper = mapper;
        _taskRepository = taskRepository;
    }

    public async Task<OperationResult<TaskWithCategoryDetailsResponse>> Handle(GetTaskById request, CancellationToken cancellationToken)
    {
        var result = new OperationResult<TaskWithCategoryDetailsResponse>();
        try
        {
            var task = await _taskRepository.GetAsync(request.TaskId, cancellationToken);
            if (task is null)
            {
                result.AddError(ErrorCode.NotFound, "No task is found.");
                return result;
            }
            var mappedTask = _mapper.Map<TaskWithCategoryDetailsResponse>(task);
            result.Payload = mappedTask;
        }
        catch (Exception ex)
        {
            result.AddError(ErrorCode.UnknownError, ex.Message);
        }

        return result;
    }
}
