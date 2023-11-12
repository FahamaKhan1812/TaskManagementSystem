using MediatR;
using Microsoft.EntityFrameworkCore;
using TaskManagementSystem.Application.Categories.Commads;
using TaskManagementSystem.Application.Models;
using TaskManagementSystem.DAL.Data;

namespace TaskManagementSystem.Application.Categories.CommandHandlers;
public class DeleteCategoryCommandHandler : IRequestHandler<DeleteCategoryCommand, OperationResult<string>>
{
    private readonly DataContext _dataContext;

    public DeleteCategoryCommandHandler(DataContext dataContext)
    {
        _dataContext = dataContext;
    }

    public async Task<OperationResult<string>> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
    {
        var result = new OperationResult<string>();
        try
        {
            var category = await _dataContext.Categories
               .FirstOrDefaultAsync(c => c.Id == request.CategoryId, cancellationToken);

            if (category == null)
            {
                result.IsError = true;
                Error error = new()
                {
                    Message = "No Category is found."
                };
                result.Errors.Add(error);
                return result;
            }

            _dataContext.Categories.Remove(category);
            await _dataContext.SaveChangesAsync(cancellationToken);
            result.Payload = "Deleted successfully";
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
