using System.ComponentModel.DataAnnotations;
using TaskManagementSystem.Domain.Enums;

namespace TaskManagementSystem.Application.Contracts.Task.Request;
public class CreateTask
{
    public Guid Id { get; private set; }
    [Required]
    public Guid UserId { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public Priority Priority { get; set; }
    public bool IsCompleted { get; set; }
    [Required]
    public Guid CategoryId { get; set; }
}
