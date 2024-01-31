using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TaskManagementSystem.Application.Categories.Commads;
using TaskManagementSystem.Application.Contracts.Category.Request;
using TaskManagementSystem.Application.Enums;
using TaskManagementSystem.Application.Models;
using TaskManagementSystem.DAL.Data;
using TaskManagementSystem.Domain.Entities;

namespace TaskManagementSystem.Application.Categories.CommandHandlers;
public class UpdateCategoryCommandHandler : IRequestHandler<UpdateCategoryCommand, OperationResult<UpdateCategory>>
{
    private readonly DataContext _dataContext;
    private readonly IMapper _mapper;

    public UpdateCategoryCommandHandler(DataContext dataContext, IMapper mapper)
    {
        _dataContext = dataContext;
        _mapper = mapper;
    }

    public async Task<OperationResult<UpdateCategory>> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
    {
        var result = new OperationResult<UpdateCategory>();
        try
        {
            var category = await _dataContext.Categories
               .FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

            if (category == null)
            {
                result.AddError(ErrorCode.NotFound, "No Category is found.");
                return result;
            }

            if (request.UserRole == UserRole.User)
            {
                result.AddError(ErrorCode.UserNotAllowed, "User is not allowed to perform this action");
                return result;
            }

            category.Name = request.Name;
            await _dataContext.SaveChangesAsync(cancellationToken);

            var mappedCategory = _mapper.Map<UpdateCategory>(category);

            result.Payload = mappedCategory;

        }
        catch (Exception ex)
        {
            result.AddError(ErrorCode.UnknownError, ex.Message);
        }

        return result;
    }
}
