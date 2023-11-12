using MediatR;
using TaskManagementSystem.Application.Models;

namespace TaskManagementSystem.Application.Categories.Commads;
public class DeleteCategoryCommand : IRequest<OperationResult<string>>
{
    public Guid CategoryId { get; set; }
}
