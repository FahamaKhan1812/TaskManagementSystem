using MediatR;
using TaskManagementSystem.Application.Contracts.Category.Request;
using TaskManagementSystem.Application.Models;

namespace TaskManagementSystem.Application.Categories.Commads;
public class CreateCategoryCommand : IRequest<OperationResult<CreateCategory>>
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public string Name { get; set; }
    public string UserRole { get; set; }
}