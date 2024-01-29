namespace TaskManagementSystem.Application.Contracts.Common;

public class ErrorResponse
{
    public ErrorResponse()
    {
        Errors = new List<string>();
    }
    
    public int StatusCode { get; set; }
    public string? StatusPhrase { get; set; }
    public List<string> Errors { get; init; } 
    public DateTime Timestamp { get; set; }
}
