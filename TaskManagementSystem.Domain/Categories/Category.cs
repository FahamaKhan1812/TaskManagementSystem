using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TaskManagementSystem.Domain.Categories;
public class Category
{
    [Key]
    public Guid Id { get; set; }
    public string Name { get; set; }

    // Navigation property for the Task relationship
    [JsonIgnore]
    public ICollection<Domain.Tasks.Task> Tasks { get; set; }
}