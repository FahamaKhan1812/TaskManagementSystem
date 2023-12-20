namespace TaskManagementSystem.Application.Contracts.Identity.Response;

public class AuthenticationResponse
{
    public string Token { get; set; }
    public string Date { get; private set; } = DateTime.Now.ToString("yyyy-MM-dd:HH:mm:ss");

}
