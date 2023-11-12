using MediatR;
using TaskManagementSystem.Application.Contracts.Category.Response;
using TaskManagementSystem.Application.Models;

namespace TaskManagementSystem.Application.Categories.Queries;
public class GetCategoryById : IRequest<OperationResult<CategoryResponse>>
{
    public Guid CategoryId { get; set; }
}
