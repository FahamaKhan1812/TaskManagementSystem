using AutoMapper;
using MediatR;
using TaskManagementSystem.Application.Categories.Commads;
using TaskManagementSystem.Application.Contracts.Category.Request;
using TaskManagementSystem.Application.Models;
using TaskManagementSystem.DAL.Data;
using TaskManagementSystem.Domain.Entities;

namespace TaskManagementSystem.Application.Categories.CommandHandlers;
public class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand, OperationResult<CreateCategory>>
{
    private readonly DataContext _dataContext;
    private readonly IMapper _mapper;

    public CreateCategoryCommandHandler(DataContext dataContext, IMapper mapper)
    {
        _dataContext = dataContext;
        _mapper = mapper;
    }

    public async Task<OperationResult<CreateCategory>> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        var result = new OperationResult<CreateCategory>();
        try
        {
            Category category = new()
            {
                Id = request.Id,
                Name = request.Name,
            };

            await _dataContext.Categories.AddAsync(category, cancellationToken);
            await _dataContext.SaveChangesAsync(cancellationToken);
            var mappedCategory = _mapper.Map<CreateCategory>(category);

            result.Payload = mappedCategory;
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