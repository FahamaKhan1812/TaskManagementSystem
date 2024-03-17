using TaskManagementSystem.Domain.Categories;
using TaskManagementSystem.Domain.Enums;
using TaskManagementSystem.Domain.Users;

namespace TaskManagementSystem.Domain.Tasks;
public class Task
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime UpdatedDate { get; set; }
    public Priority Priority { get; set; }
    public bool IsCompleted { get; set; }
    public bool IsEmailSent { get; set; }
    public DateTime? ReminderDateTime { get; set; }

    // Navigation property for the Category relationship
    public Guid CategoryId { get; set; }
    public Category Category { get; set; }

    // Navigation property for the User relationship
    public Guid UserProfileId { get; set; }
    public User User { get; set; }
}
