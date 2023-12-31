using MediatR;
using TaskManagementSystem.Application.Contracts.Task.Request;
using TaskManagementSystem.Application.Models;
using TaskManagementSystem.Domain.Enums;

namespace TaskManagementSystem.Application.Tasks.Commands;
public class UpdateTaskCommand : IRequest<OperationResult<UpdateTask>>
{
    public Guid TaskId { get; set; }
    public Guid UserId { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public bool IsCompleted { get; set; }
    public Priority Priority { get; set; }
    public Guid CategoryId { get; set; }
    public DateTime UpdatedDate { get; private set; } = DateTime.Now;
}
