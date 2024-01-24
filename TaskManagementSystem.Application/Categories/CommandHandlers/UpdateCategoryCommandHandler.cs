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
                result.IsError = true;
                Error error = new()
                {
                    Code = ErrorCode.NotFound,
                    Message = "No Category is found."
                };
                result.Errors.Add(error);
                return result;
            }

            if (request.UserRole == UserRole.User)
            {
                result.IsError = true;
                Error error = new()
                {
                    Code = ErrorCode.UserNotAllowed,
                    Message = "User is not allowed to perform this action"
                };
                result.Errors.Add(error);
                return result;
            }

            category.Name = request.Name;
            await _dataContext.SaveChangesAsync(cancellationToken);

            var mappedCategory = _mapper.Map<UpdateCategory>(category);

            result.Payload = mappedCategory;

        }
        catch (Exception ex)
        {
            result.IsError = true;
            Error erros = new()
            {
                Code = ErrorCode.UnknownError,
                Message = ex.Message
            };

            result.Errors.Add(erros);
        }

        return result;
    }
}
