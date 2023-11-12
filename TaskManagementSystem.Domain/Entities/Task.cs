using TaskManagementSystem.Domain.Enums;

namespace TaskManagementSystem.Domain.Entities;
public class Task
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime CreatedDate { get; set;}
    public DateTime UpdatedDate { get; set;}
    public Priority Priority { get; set; }
    public bool IsCompleted { get; set; }

    // Navigation property for the Category relationship
    public Guid CategoryId { get; set; }
    public Category Category { get; set; }
}
