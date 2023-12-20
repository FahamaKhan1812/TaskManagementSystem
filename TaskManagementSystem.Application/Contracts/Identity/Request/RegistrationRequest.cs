using System.ComponentModel.DataAnnotations;

namespace TaskManagementSystem.Application.Contracts.Identity.Request;
public class RegistrationRequest
{
    [Required]
    [EmailAddress]
    public string UserName { get; set; }
    [Required]
    public string Password { get; set; }

    [Required]
    [MinLength(3)]
    [MaxLength(50)]
    public string FirstName { get; set; }

    [Required]
    [MinLength(3)]
    [MaxLength(50)]
    public string LastName { get; set; }
}
