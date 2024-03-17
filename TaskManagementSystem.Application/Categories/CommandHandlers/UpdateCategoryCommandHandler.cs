using AutoMapper;
using MediatR;
using System.Linq.Expressions;
using TaskManagementSystem.Application.Categories.Commads;
using TaskManagementSystem.Application.Contracts.Category.Request;
using TaskManagementSystem.Application.Enums;
using TaskManagementSystem.Application.Models;
using TaskManagementSystem.Domain.Categories;

namespace TaskManagementSystem.Application.Categories.CommandHandlers;
internal sealed class UpdateCategoryCommandHandler : IRequestHandler<UpdateCategoryCommand, OperationResult<UpdateCategory>>
{
    private readonly IMapper _mapper;
    private readonly ICategoryRepository _categoryRepository;

    public UpdateCategoryCommandHandler(IMapper mapper, ICategoryRepository categoryRepository)
    {
        _mapper = mapper;
        _categoryRepository = categoryRepository;
    }

    public async Task<OperationResult<UpdateCategory>> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
    {
        var result = new OperationResult<UpdateCategory>();
        try
        {
            var isUserAdmin = _categoryRepository.IsUserAdmin(request.UserRole);
            if (!isUserAdmin)
            {
                result.AddError(ErrorCode.UserNotAllowed, "User is not allowed to do specific operation");
                return result;
            }
            var category = await _categoryRepository.GetAsync(new Expression<Func<Category, bool>>[] { c => c.Id == request.Id }, cancellationToken);

            if (category == null)
            {
                result.AddError(ErrorCode.NotFound, "No Category is found.");
                return result;
            }

            category.Name = request.Name;
            await _categoryRepository.UpdateAsync(category, cancellationToken);
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
