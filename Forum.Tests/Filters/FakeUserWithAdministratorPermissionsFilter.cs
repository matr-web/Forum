using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace Forum.Tests.Filters;

public class FakeUserWithAdministratorPermissionsFilter : IAsyncActionFilter
{
    /// <summary>
    /// Mocking User Claims with administrator permissions.
    /// </summary>
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var claimsPrincipal = new ClaimsPrincipal();

        // Add needed Claims. 
        claimsPrincipal.AddIdentity(new ClaimsIdentity(
            new[]
            {
                new Claim(ClaimTypes.NameIdentifier, "6233bef7-5450-4203-26dc-08daa633fcb0"),
                new Claim(ClaimTypes.Role, "Administrator"),
            }));

        context.HttpContext.User = claimsPrincipal;

        await next();
    }
}
