using System.ComponentModel.DataAnnotations;

namespace TaskManagementSystem.Application.Contracts.Identity.Request;
public class UpdateIdentityRequest
{
    [Required]
    [EmailAddress]
    public string UserName { get; set; }

    [Required]
    [MinLength(3)]
    [MaxLength(50)]
    public string FirstName { get; set; }

    [Required]
    [MinLength(3)]
    [MaxLength(50)]
    public string LastName { get; set; }

}
