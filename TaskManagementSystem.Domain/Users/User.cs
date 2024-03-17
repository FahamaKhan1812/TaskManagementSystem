using System.ComponentModel.DataAnnotations;

namespace TaskManagementSystem.Domain.Users;
public class User
{
    [Key]
    public Guid UserId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime UpdatedDate { get; set; }
    public string Role { get; set; }
    public ICollection<Tasks.Task> Tasks { get; set; }
}
