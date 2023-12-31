using MediatR;
using Microsoft.AspNetCore.Identity;
using TaskManagementSystem.Application.Enums;
using TaskManagementSystem.Application.Identity.Commands;
using TaskManagementSystem.Application.Models;
using TaskManagementSystem.DAL.Data;
using TaskManagementSystem.Domain.Entities;

namespace TaskManagementSystem.Application.Identity.CommandHandlers;
public class RegisterCommandHandler : IRequestHandler<RegisterCommand, OperationResult<string>>
{
    private readonly DataContext _dataContext;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    public RegisterCommandHandler(DataContext dataContext, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        _dataContext = dataContext;
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task<OperationResult<string>> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        var result = new OperationResult<string>();

        try
        {
            var existingIdentity = await _userManager.FindByEmailAsync(request.Username);

            // Check if the user is already exist or not
            if (existingIdentity != null)
            {
                result.IsError = true;
                Error error = new()
                {
                    Code = ErrorCode.IdentityUserAlreadyExists,
                    Message = "Provided informations is already exists."
                };
                result.Errors.Add(error);
                return result;
            }

            // Creating a new User
            IdentityUser identity = new()
            {
                Email = request.Username,
                UserName = request.Username
            };

            //creating transaction
            using var transaction = _dataContext.Database.BeginTransaction();
            var createdIdentity = await _userManager.CreateAsync(identity, request.Password);
            if (!createdIdentity.Succeeded)
            {
                await transaction.RollbackAsync(cancellationToken);
                result.IsError = true;
                foreach (var identityErrors in createdIdentity.Errors)
                {
                    Error error = new()
                    {
                        Code = ErrorCode.IdentityCreationFailed,
                        Message = identityErrors.Description
                    };
                    result.Errors.Add(error);
                }
                return result;
            }

            User userInfo = new()
            {
                UserId = Guid.Parse(identity.Id),
                Email = request.Username,
                FirstName = request.FirstName,
                LastName = request.LastName,
                Role = "User",
                CreatedDate = DateTime.UtcNow,
                UpdatedDate = DateTime.UtcNow
            };

            try
            {
                // For assinging user Admin Role.
                //if (!await _roleManager.RoleExistsAsync(UserRole.Admin))
                //    await _roleManager.CreateAsync(new IdentityRole(UserRole.Admin));

                //if (await _roleManager.RoleExistsAsync(UserRole.Admin))
                //{
                //    await _userManager.AddToRoleAsync(identity, UserRole.Admin);
                //}
                _dataContext.Add(userInfo);
                await _dataContext.SaveChangesAsync(cancellationToken);
                await transaction.CommitAsync(cancellationToken);
            }
            catch (Exception)
            {
                await transaction.RollbackAsync(cancellationToken);
                throw;
            }

            result.Payload = "User is Created Successfully";
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
