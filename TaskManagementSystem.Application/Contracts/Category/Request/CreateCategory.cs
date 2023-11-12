namespace TaskManagementSystem.Application.Contracts.Category.Request;
public class CreateCategory
{
    public Guid Id { get; private set; }
    public string Name { get; set; }
}
