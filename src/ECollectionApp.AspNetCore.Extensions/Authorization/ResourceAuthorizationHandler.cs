using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ECollectionApp.AspNetCore.Microservice.Authorization
{
    public abstract class ResourceAuthorizationHandler<TResource> : AuthorizationHandler<OperationAuthorizationRequirement, TResource>
    {
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, OperationAuthorizationRequirement requirement, TResource resource)
        {
            bool authorized = await AuthorizeAsync(context.User, resource, requirement.Name);
            if (authorized)
            {
                context.Succeed(requirement);
            }
            else
            {
                context.Fail();
            }
        }

        protected abstract Task<bool> AuthorizeAsync(ClaimsPrincipal user, TResource resource, string operation);
    }
}
