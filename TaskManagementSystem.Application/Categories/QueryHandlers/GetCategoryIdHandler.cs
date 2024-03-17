using AutoMapper;
using MediatR;
using System.Linq.Expressions;
using TaskManagementSystem.Application.Categories.Queries;
using TaskManagementSystem.Application.Contracts.Category.Response;
using TaskManagementSystem.Application.Enums;
using TaskManagementSystem.Application.Models;
using TaskManagementSystem.Domain.Categories;

namespace TaskManagementSystem.Application.Categories.QueryHandlers;

internal sealed class GetCategoryIdHandler : IRequestHandler<GetCategoryById, OperationResult<CategoryResponse>>
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IMapper _mapper;
    public GetCategoryIdHandler(IMapper mapper, ICategoryRepository categoryRepository)
    {
        _mapper = mapper;
        _categoryRepository = categoryRepository;
    }

    public async Task<OperationResult<CategoryResponse>> Handle(GetCategoryById request, CancellationToken cancellationToken)
    {
        var result = new OperationResult<CategoryResponse>();
        try
        {
            var category = await _categoryRepository.GetAsync(new Expression<Func<Category, bool>>[] { c => c.Id == request.CategoryId }, cancellationToken);

            if (category == null)
            {
                result.AddError(ErrorCode.NotFound, "No Category is found.");
                return result;
            }
            var mappedCategory = _mapper.Map<CategoryResponse>(category);
            result.Payload = mappedCategory;

        }
        catch (Exception ex)
        {
            result.AddError(ErrorCode.UnknownError, ex.Message);
        }

        return result;
    }
}
