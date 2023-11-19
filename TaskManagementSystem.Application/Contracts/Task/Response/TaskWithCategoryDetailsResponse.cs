namespace TaskManagementSystem.Application.Contracts.Task.Response;
public class TaskWithCategoryDetailsResponse
{

    public Guid TaskId { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime UpdatedDate { get; set; }
    public string Priority { get; private set; }
    public bool IsCompleted { get; set; }

    // Details of the associated category
    public Domain.Entities.Category Category { get; set; }
}

