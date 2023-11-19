using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TaskManagementSystem.Application.Categories.Queries;
using TaskManagementSystem.Application.Contracts.Category.Response;
using TaskManagementSystem.Application.Enums;
using TaskManagementSystem.Application.Models;
using TaskManagementSystem.DAL.Data;

namespace TaskManagementSystem.Application.Categories.QueryHandlers;
public class GetCategoryIdHandler : IRequestHandler<GetCategoryById, OperationResult<CategoryResponse>>
{
    private readonly DataContext _dataContext;
    private readonly IMapper _mapper;
    public GetCategoryIdHandler(DataContext dataContext, IMapper mapper)
    {
        _dataContext = dataContext;
        _mapper = mapper;
    }

    public async Task<OperationResult<CategoryResponse>> Handle(GetCategoryById request, CancellationToken cancellationToken)
    {
        var result = new OperationResult<CategoryResponse>();
		try
		{
            var category = await _dataContext.Categories
                .FirstOrDefaultAsync(c => c.Id == request.CategoryId, cancellationToken);

            if(category == null)
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
            var mappedCategory = _mapper.Map<CategoryResponse>(category);
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
