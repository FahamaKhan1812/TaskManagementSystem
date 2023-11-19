using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TaskManagementSystem.Domain.Entities;
public class Category
{
    [Key]
    public Guid Id { get; set; }
    public string Name { get; set; }

    // Navigation property for the Task relationship
    [JsonIgnore]
    public ICollection<Task> Tasks { get; set; }
}