using System.Linq.Expressions;
using MediatR;
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
        try
        {
            var category = await _categoryRepository.GetAsync(new Expression<Func<Category, bool>>[] { c => c.Id == request.CategoryId }, cancellationToken);

            if (category == null)
            {
                return OperationResult<string>.Failure(ErrorCode.NotFound,"No Category is found.");
            }
            if (request.UserRole == UserRole.User)
            {
                return OperationResult<string>.Failure(ErrorCode.UserNotAllowed,"User is not allowed to perform this action");
            }

            await _categoryRepository.DeleteAsync(category, cancellationToken);
        }
        catch (Exception ex)
        {
            return OperationResult<string>.Failure(ErrorCode.UnknownError,ex.Message);
        }
        return OperationResult<string>.Success(ErrorCode.Ok, "Deleted successfully");
    }
}
