using TaskManagementSystem.Application.Enums;

namespace TaskManagementSystem.Application.Models;
public class Error
{
    public ErrorCode Code { get; set; }
    public string? Message { get; set; }
}
