using Domain.CustomExceptions;
using System.Security.Claims;

namespace starter_project_template.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static Guid GetUserId(this ClaimsPrincipal principal)
        {
            string? strUserId = principal.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!Guid.TryParse(strUserId, out var parsed))
            {
                throw new InvalidAccessTokenException("User ID is missing or invalid!");
            }

            return parsed;
        }
    }
}
