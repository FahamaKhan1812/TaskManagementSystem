namespace TaskManagementSystem.Api;
public class ApiRoutes
{
    public const string BaseRoute = "/api/v{version:apiVersion}/[controller]";

    public class Identity
    {
        public const string RegisterNewIdentity = "register";
        public const string LoginIdentity = "login";
        public const string IdRoute = "{identityId}";
    }
    public class Tasks
    {
        public const string IdRoute = "{id}";
    }
}
