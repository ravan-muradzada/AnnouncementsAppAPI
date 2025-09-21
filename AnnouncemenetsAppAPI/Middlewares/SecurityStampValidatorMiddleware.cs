using Domain.Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AnnouncemenetsAppAPI.Middlewares
{
    public class SecurityStampValidatorMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IServiceProvider _serviceProvider;

        public SecurityStampValidatorMiddleware(RequestDelegate next, IServiceProvider serviceProvider)
        {
            _next = next;
            _serviceProvider = serviceProvider;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Skip if endpoint does not have Authorize attribute
            var endpoint = context.GetEndpoint();
            if (endpoint == null || !endpoint.Metadata.Any(m => m is Microsoft.AspNetCore.Authorization.AuthorizeAttribute))
            {
                await _next(context);
                return;
            }
            if (context.User.Identity?.IsAuthenticated == true)
            {
                var userId = context.User.FindFirstValue(ClaimTypes.NameIdentifier);
                var tokenStamp = context.User.FindFirstValue("security_stamp");

                if (!string.IsNullOrEmpty(userId) && !string.IsNullOrEmpty(tokenStamp))
                {
                    using var scope = _serviceProvider.CreateScope(); // <-- create scoped provider
                    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                    var user = await userManager.FindByIdAsync(userId);

                    if (user != null && user.SecurityStamp != tokenStamp)
                    {
                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        // Token expired due to password reset or security change.
                        return;
                    }
                }
            }

            await _next(context);
        }
    }

    public static class SecurityStampValidatorMiddlewareExtensions
    {
        public static IApplicationBuilder UseSecurityStampValidatorMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<SecurityStampValidatorMiddleware>();
        }
    }
}
