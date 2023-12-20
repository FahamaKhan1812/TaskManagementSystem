namespace TaskManagementSystem.Application.Options;
public class JwtSetting
{
    public string Secret { get; set; }
    public string Issuer { get; set; }
    public string[] Audiences { get; set; }
}
