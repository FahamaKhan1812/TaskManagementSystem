using MediatR;
using TaskManagementSystem.Application.Contracts.Task.Request;
using TaskManagementSystem.Application.Models;
using TaskManagementSystem.Domain.Enums;

namespace TaskManagementSystem.Application.Tasks.Commands;
public class CreateTaskCommand : IRequest<OperationResult<CreateTask>>
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime CreatedDate { get; private set; } = DateTime.Now;
    public DateTime UpdatedDate { get; private set; } = DateTime.Now;
    public Priority Priority { get; set; }
    public bool IsCompleted { get; set; }
    public Guid CategoryId { get; set; }
}
