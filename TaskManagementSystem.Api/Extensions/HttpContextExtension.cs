using System.Security.Claims;

namespace TaskManagementSystem.Api.Extensions;
public static class HttpContextExtensions
{
    public static string? GetUserRole(this HttpContext context)
    {
        // Make sure the ClaimsIdentity exists before attempting to retrieve roles
        if (context.User.Identity is ClaimsIdentity identity)
        {
            var roleClaim = identity.FindFirst(ClaimTypes.Role);
            return roleClaim?.Value;
        }

        return null;
    }
    public static Guid GetUserProfileIdClaimValue(this HttpContext context)
    {
        var userProfileId = GetClaimValue(context, "UserId");
        return userProfileId;
    }
    private static Guid GetClaimValue(this HttpContext context, string key)
    {
        var identity = context.User.Identity as ClaimsIdentity;

        // Make sure the claim exists before attempting to parse its value
        var claim = identity?.FindFirst(key);

        if (claim != null && Guid.TryParse(claim.Value, out var claimValue))
        {
            return claimValue;
        }

        // Handle the case where the claim is not found or cannot be parsed
        throw new InvalidOperationException($"Claim '{key}' not found or has an invalid value.");
    }
}
