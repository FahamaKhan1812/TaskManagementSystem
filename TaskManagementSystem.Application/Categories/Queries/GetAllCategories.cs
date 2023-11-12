using MediatR;
using TaskManagementSystem.Application.Contracts.Category.Response;
using TaskManagementSystem.Application.Models;


namespace TaskManagementSystem.Application.Categories.Queries;
public class GetAllCategories : IRequest<OperationResult<List<CategoryResponse>>>
{
}
