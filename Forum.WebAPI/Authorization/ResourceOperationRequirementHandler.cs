using Forum.Entities;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace Forum.WebAPI.Authorization;

public class ResourceOperationRequirementHandler : AuthorizationHandler<ResourceOperationRequirement, Resource>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ResourceOperationRequirement requirement, Resource resource)
    {
        // Everyone can access Read and Create operations. 
        if (requirement.ResourceOperation == ResourceOperation.Read || requirement.ResourceOperation == ResourceOperation.Create)
        {
            context.Succeed(requirement);
        }

        Guid userId = Guid.Parse(context.User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier).Value);

        // Only Author and Administrator can access Update and Delete operations on Question entity.
        if (resource.question is not null && resource.question.AuthorId == userId || resource.question is not null && context.User.IsInRole("Administrator"))
        {
            context.Succeed(requirement);
        }

        // Only Author and Administrator can access Update and Delete operations on Answer entity.
        if (resource.answer is not null && resource.answer.AuthorId == userId || resource.answer is not null && context.User.IsInRole("Administrator"))
        {
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }
}




