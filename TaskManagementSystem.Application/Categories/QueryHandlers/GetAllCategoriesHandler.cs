using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TaskManagementSystem.Application.Categories.Queries;
using TaskManagementSystem.Application.Contracts.Category.Response;
using TaskManagementSystem.Application.Models;
using TaskManagementSystem.DAL.Data;
using TaskManagementSystem.Domain.Entities;

namespace TaskManagementSystem.Application.Categories.QueryHandlers;
public class GetAllCategoriesHandler : IRequestHandler<GetAllCategories, OperationResult<List<CategoryResponse>>>
{
    private readonly DataContext _dataContext;
    private readonly IMapper _mapper;

    public GetAllCategoriesHandler(DataContext dataContext, IMapper mapper)
    {
        _dataContext = dataContext;
        _mapper = mapper;
    }

    public async Task<OperationResult<List<CategoryResponse>>> Handle(GetAllCategories request, CancellationToken cancellationToken)
    {
        var result = new OperationResult<List<CategoryResponse>>();

        try
        {
            var categories= await _dataContext.Categories.ToListAsync(cancellationToken);
            var mappedcategories = _mapper.Map<List<CategoryResponse>>(categories);
            result.Payload = mappedcategories;
        }
        catch (Exception ex)
        {
            result.IsError = true;
            Error erros = new()
            {
                Message = ex.Message
            };

            result.Errors.Add(erros);
        }
        return result;
    }
}
