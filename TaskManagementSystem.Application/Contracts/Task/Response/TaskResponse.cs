namespace TaskManagementSystem.Application.Contracts.Task.Response;

public class TaskResponse
{
    public List<TaskWithCategoryDetailsResponse> Tasks { get; set; }
    public int CurrentPage { get; set; }
    public int Pages { get; set; }
}
