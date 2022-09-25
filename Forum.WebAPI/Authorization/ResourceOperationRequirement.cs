using Microsoft.AspNetCore.Authorization;

namespace Forum.WebAPI.Authorization;

public enum ResourceOperation
{
    Read,
    Create,
    Update,
    Delete
}

public class ResourceOperationRequirement : IAuthorizationRequirement
{
    public ResourceOperationRequirement(ResourceOperation resourceOperation)
    {
        ResourceOperation = resourceOperation;
    }

    public ResourceOperation ResourceOperation { get; }

}
