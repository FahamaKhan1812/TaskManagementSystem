using AutoMapper;
using MediatR;
using TaskManagementSystem.Application.Categories.Queries;
using TaskManagementSystem.Application.Contracts.Category.Response;
using TaskManagementSystem.Application.Enums;
using TaskManagementSystem.Application.Models;
using TaskManagementSystem.Domain.Categories;

namespace TaskManagementSystem.Application.Categories.QueryHandlers;
internal sealed class GetAllCategoriesHandler : IRequestHandler<GetAllCategories, OperationResult<List<CategoryResponse>>>
{
    private readonly IMapper _mapper;
    private readonly ICategoryRepository _categoryRepository;

    public GetAllCategoriesHandler(IMapper mapper, ICategoryRepository categoryRepository)
    {
        _mapper = mapper;
        _categoryRepository = categoryRepository;
    }

    public async Task<OperationResult<List<CategoryResponse>>> Handle(GetAllCategories request, CancellationToken cancellationToken)
    {
        var result = new OperationResult<List<CategoryResponse>>();

        try
        {
            var categories = await _categoryRepository.GetAllAsync(cancellationToken);
            var mappedcategories = _mapper.Map<List<CategoryResponse>>(categories);
            result.Payload = mappedcategories;
        }
        catch (Exception ex)
        {
            result.AddError(ErrorCode.UnknownError, ex.Message);
        }
        return result;
    }
}
