using AutoMapper;
using MediatR;
using System.Linq.Expressions;
using TaskManagementSystem.Application.Contracts.Task.Request;
using TaskManagementSystem.Application.Enums;
using TaskManagementSystem.Application.Models;
using TaskManagementSystem.Application.Tasks.Commands;
using TaskManagementSystem.Domain.Tasks;

namespace TaskManagementSystem.Application.Tasks.CommandHandlers;
internal sealed class UpdateTaskCommandHandler : IRequestHandler<UpdateTaskCommand, OperationResult<UpdateTask>>
{
    private readonly ITaskRepository _taskRepository;
    private readonly IMapper _mapper;

    public UpdateTaskCommandHandler(IMapper mapper, ITaskRepository taskRepository)
    {
        _mapper = mapper;
        _taskRepository = taskRepository;
    }

    public async Task<OperationResult<UpdateTask>> Handle(UpdateTaskCommand request, CancellationToken cancellationToken)
    {
        var result = new OperationResult<UpdateTask>();
        try
        {
            var task = await _taskRepository.GetAsync(new Expression<Func<Domain.Tasks.Task, bool>>[] { c => c.Id == request.TaskId }, cancellationToken);
            if (task is null)
            {
                result.AddError(ErrorCode.NotFound, "No task is found.");
                return result;
            }
            if (task.UserProfileId != request.UserId)
            {
                result.AddError(ErrorCode.UserNotAllowed, "User is not allowed to do specific operation");
                return result;
            }
            task.Title = request.Title;
            task.Description = request.Description;
            task.IsCompleted = request.IsCompleted;
            task.UpdatedDate = request.UpdatedDate;
            task.Priority = request.Priority;
            task.CategoryId = request.CategoryId;

            await _taskRepository.UpdateAsync(task, cancellationToken);
            var mappedTask = _mapper.Map<UpdateTask>(task);
            result.Payload = mappedTask;
        }
        catch (Exception ex)
        {
            result.AddError(ErrorCode.UnknownError, ex.Message);
        }

        return result;
    }
}
