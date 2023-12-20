using MediatR;
using TaskManagementSystem.Application.Models;

namespace TaskManagementSystem.Application.Identity.Commands;
public class RegisterCommand : IRequest<OperationResult<string>>
{
    public string Username { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Password { get; set; }
    public string Email { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime UpdatedDate { get; set; }
}
