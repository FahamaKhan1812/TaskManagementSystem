using MediatR;
using System.Linq.Expressions;
using TaskManagementSystem.Application.Enums;
using TaskManagementSystem.Application.Models;
using TaskManagementSystem.Application.Tasks.Commands;
using TaskManagementSystem.Domain.Tasks;

namespace TaskManagementSystem.Application.Tasks.CommandHandlers;
internal sealed class DeleteTaskCommandHandler : IRequestHandler<DeleteTaskCommand, OperationResult<string>>
{
    private readonly ITaskRepository _taskRepository;

    public DeleteTaskCommandHandler(ITaskRepository taskRepository)
    {
        _taskRepository = taskRepository;
    }

    public async Task<OperationResult<string>> Handle(DeleteTaskCommand request, CancellationToken cancellationToken)
    {
        var result = new OperationResult<string>();
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
            await _taskRepository.DeleteAsync(task, cancellationToken);
            result.Payload = "Deleted successfully";
        }
        catch (Exception ex)
        {
            result.AddError(ErrorCode.UnknownError, ex.Message);
        }

        return result;
    }
}
