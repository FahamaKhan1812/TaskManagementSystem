using MediatR;
using Microsoft.EntityFrameworkCore;
using TaskManagementSystem.Application.Enums;
using TaskManagementSystem.Application.Identity.Commands;
using TaskManagementSystem.Application.Models;
using TaskManagementSystem.DAL.Data;
using TaskManagementSystem.Domain.Entities;

namespace TaskManagementSystem.Application.Identity.CommandHandlers;
internal class UpdateIdentityCommandHandler : IRequestHandler<UpdateIdentityCommand, OperationResult<string>>
{
    private readonly DataContext _dataContext;
    public UpdateIdentityCommandHandler(DataContext dataContext)
    {
        _dataContext = dataContext;
    }
    public async Task<OperationResult<string>> Handle(UpdateIdentityCommand request, CancellationToken cancellationToken)
    {
        var result = new OperationResult<string>();
        try
        {
            if (request.UserRole != UserRole.Admin)
            {
                result.IsError = true;
                Error error = new()
                {
                    Code = ErrorCode.UserNotAllowed,
                    Message = "User is not allowed to do specific operation"
                };
                result.Errors.Add(error);
                return result;
            }

            var user = await _dataContext.Users.FirstOrDefaultAsync(u => u.UserId == request.UserId, cancellationToken);
            if(user == null)
            {
                result.IsError = true;
                Error error = new()
                {
                    Code = ErrorCode.NotFound,
                    Message = "No User is found"
                };
                result.Errors.Add(error);
                return result;
            }

            user.Email = request.UserName;
            user.FirstName = request.FirstName; 
            user.LastName = request.LastName;
            user.UpdatedDate = request.UpdatedAt;
            
            _dataContext.Users.Update(user);
            await _dataContext.SaveChangesAsync(cancellationToken);
            result.Payload = "User updated successfully";

        }
        catch (Exception ex)
        {
            result.IsError = true;
            Error errors = new()
            {
                Code = ErrorCode.UnknownError,
                Message = ex.Message
            };
            result.Errors.Add(errors);
        }
        return result;
    }
}
