using MediatR;
using TaskManagementSystem.Application.Contracts.Category.Request;
using TaskManagementSystem.Application.Models;

namespace TaskManagementSystem.Application.Categories.Commads;
public class UpdateCategoryCommand : IRequest<OperationResult<UpdateCategory>>
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string UserRole { get; set; }
}
