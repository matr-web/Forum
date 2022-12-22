using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace Forum.Tests.Filters;

public class FakeUserFilter : IAsyncActionFilter
{
    /// <summary>
    /// Mocking User Claims.
    /// </summary>
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var claimsPrincipal = new ClaimsPrincipal();

        // Add needed Claims.
        claimsPrincipal.AddIdentity(new ClaimsIdentity(
            new[]
            {
            new Claim(ClaimTypes.NameIdentifier, "9cb36c55-bf16-42bc-9722-08da9bf1eadb"),
            new Claim(ClaimTypes.Role, "User"),
            }));

        context.HttpContext.User = claimsPrincipal;

        await next();
    }
}