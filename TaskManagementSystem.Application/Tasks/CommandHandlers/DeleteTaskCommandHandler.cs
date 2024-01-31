using MediatR;
using Microsoft.EntityFrameworkCore;
using TaskManagementSystem.Application.Enums;
using TaskManagementSystem.Application.Models;
using TaskManagementSystem.Application.Tasks.Commands;
using TaskManagementSystem.DAL.Data;

namespace TaskManagementSystem.Application.Tasks.CommandHandlers;
internal class DeleteTaskCommandHandler : IRequestHandler<DeleteTaskCommand, OperationResult<string>>
{
    private readonly DataContext _dataContext;

    public DeleteTaskCommandHandler(DataContext dataContext)
    {
        _dataContext = dataContext;
    }

    public async Task<OperationResult<string>> Handle(DeleteTaskCommand request, CancellationToken cancellationToken)
    {
        var result = new OperationResult<string>();
        try
        {
            var task = await _dataContext.Tasks.FirstOrDefaultAsync(t => t.Id == request.TaskId, cancellationToken);
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
            _dataContext.Remove(task);
            await _dataContext.SaveChangesAsync(cancellationToken);
            result.Payload = "Deleted successfully";
        }
        catch (Exception ex)
        {
            result.AddError(ErrorCode.UnknownError, ex.Message);
        }

        return result;
    }
}
