using System.ComponentModel.DataAnnotations;

namespace TaskManagementSystem.Application.Contracts.Identity.Request;
public class LoginRequest
{
    [Required]
    [EmailAddress]
    public string UserName { get; set; }
    [Required]
    public string Password { get; set; }
}
