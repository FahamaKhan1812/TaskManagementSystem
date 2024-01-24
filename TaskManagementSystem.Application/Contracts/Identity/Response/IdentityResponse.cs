namespace TaskManagementSystem.Application.Contracts.Identity.Response;
public class IdentityResponse
{
    public Guid Id { get; set; }
    public string FullName { get; set; }
    public string Email { get; set; }
    public string UserRole { get; set; }
    public DateTime UserCreatedAt { get; set; }
    public DateTime UserUpdatedAt { get; set; }
}
