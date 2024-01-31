using System.Linq.Expressions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManagementSystem.Application.Categories.Commads;
using TaskManagementSystem.Application.Enums;
using TaskManagementSystem.Application.Models;
using TaskManagementSystem.DAL.Data;
using TaskManagementSystem.Domain.Entities;
using TaskManagementSystem.Domain.IRepositories;

namespace TaskManagementSystem.Application.Categories.CommandHandlers;
public class DeleteCategoryCommandHandler : IRequestHandler<DeleteCategoryCommand, OperationResult<string>>
{
    private readonly ICategoryRepository _categoryRepository;

    public DeleteCategoryCommandHandler(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<OperationResult<string>> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
    {
        var result = new OperationResult<string>();
        try
        {
            var category = await _categoryRepository.GetAsync(new Expression<Func<Category, bool>>[] { c => c.Id == request.CategoryId }, cancellationToken);

            if (category == null)
            {
                result.AddError(ErrorCode.NotFound, "No Category is found.");
                return result;
            }
            if (request.UserRole == UserRole.User)
            {
                result.AddError(ErrorCode.UserNotAllowed, "User is not allowed to do specific operation");
                return result;
            }

            await _categoryRepository.DeleteAsync(category, cancellationToken);
        }
        catch (Exception ex)
        {
            result.AddError(ErrorCode.UnknownError, ex.Message); 
            return result;
        }
        return result;
    }
}
