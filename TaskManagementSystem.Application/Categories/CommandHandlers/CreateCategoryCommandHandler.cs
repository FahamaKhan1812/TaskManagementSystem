using AutoMapper;
using MediatR;
using TaskManagementSystem.Application.Categories.Commads;
using TaskManagementSystem.Application.Contracts.Category.Request;
using TaskManagementSystem.Application.Enums;
using TaskManagementSystem.Application.Models;
using TaskManagementSystem.Domain.Categories;

namespace TaskManagementSystem.Application.Categories.CommandHandlers;
internal sealed class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand, OperationResult<CreateCategory>>
{
    private readonly IMapper _mapper;
    private readonly ICategoryRepository _categoryRepository;

    public CreateCategoryCommandHandler(IMapper mapper, ICategoryRepository categoryRepository)
    {
        _mapper = mapper;
        _categoryRepository = categoryRepository;
    }

    public async Task<OperationResult<CreateCategory>> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        var result = new OperationResult<CreateCategory>();
        try
        {
            var isUserAdmin = _categoryRepository.IsUserAdmin(request.UserRole);
            if (!isUserAdmin)
            {
                result.AddError(ErrorCode.UserNotAllowed, "User is not allowed to do specific operation");
                return result;
            }
            Category category = new()
            {
                Id = request.Id,
                Name = request.Name,
            };
            await _categoryRepository.AddAsync(category, cancellationToken);
            var mappedCategory = _mapper.Map<CreateCategory>(category);
            result.Payload = mappedCategory;
        }
        catch (Exception ex)
        {
            result.AddError(ErrorCode.UnknownError, ex.Message);
        }

        return result;
    }
}