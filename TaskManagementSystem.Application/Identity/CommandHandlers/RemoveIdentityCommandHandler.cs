using MediatR;
using Microsoft.EntityFrameworkCore;
using TaskManagementSystem.Application.Enums;
using TaskManagementSystem.Application.Identity.Commands;
using TaskManagementSystem.Application.Models;
using TaskManagementSystem.DAL.Data;
using TaskManagementSystem.Domain.Entities;

namespace TaskManagementSystem.Application.Identity.CommandHandlers;
internal class RemoveIdentityCommandHandler : IRequestHandler<RemoveIdentityCommand, OperationResult<string>>
{
    private readonly DataContext _dataContext;

    public RemoveIdentityCommandHandler(DataContext dataContext)
    {
        _dataContext = dataContext;
    }

    public async Task<OperationResult<string>> Handle(RemoveIdentityCommand request, CancellationToken cancellationToken)
    {
        var result = new OperationResult<string>();
        try
        {
            if (request.UserRole != UserRole.Admin)
            {
                result.AddError(ErrorCode.UserNotAllowed, "User is not allowed to do specific operation");
                return result;
            }

            var user = await _dataContext.Users.FirstOrDefaultAsync(u => u.UserId == request.UserId, cancellationToken);
            if (user == null)
            {
                result.AddError(ErrorCode.NotFound, "No User is found");
                return result;
            }

            _dataContext.Users.Remove(user);
            await _dataContext.SaveChangesAsync(cancellationToken);
            result.Payload = "Deleted";
        }
        catch (Exception ex)
        {
            result.AddError(ErrorCode.UnknownError, ex.Message);
        }
        return result;
    }
}
