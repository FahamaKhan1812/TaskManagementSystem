namespace TaskManagementSystem.Application.Models;
public class OperationResult<T>
{
    public bool IsError { get; set; }
    public T Payload { get; set; }
    public List<Error> Errors { get; set; } = new List<Error>();
}
