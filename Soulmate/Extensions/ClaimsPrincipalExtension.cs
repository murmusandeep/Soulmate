using System.Security.Claims;

namespace Soulmate.Extensions
{
    public static class ClaimsPrincipalExtension
    {
        public static string GetUsername(this ClaimsPrincipal user)
        {
            return user?.FindFirst(ClaimTypes.NameIdentifier)?.Value
                   ?? throw new UnauthorizedAccessException("Username claim not found.");
        }

        public static int GetUserId(this ClaimsPrincipal user)
        {
            var claimValue = user?.FindFirst("UserId")?.Value;

            if (int.TryParse(claimValue, out int userId))
            {
                return userId;
            }

            throw new UnauthorizedAccessException("User ID claim not found or invalid.");
        }
    }
}
