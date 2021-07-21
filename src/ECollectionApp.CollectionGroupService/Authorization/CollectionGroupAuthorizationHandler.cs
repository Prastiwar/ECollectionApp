using ECollectionApp.AspNetCore.Microservice.Authorization;
using ECollectionApp.CollectionGroupService.Data;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ECollectionApp.CollectionGroupService.Authorization
{
    public class CollectionGroupAuthorizationHandler : ResourceAuthorizationHandler<CollectionGroup>
    {
        protected override Task<bool> AuthorizeAsync(ClaimsPrincipal user, CollectionGroup resource, string operation)
        {
            int id = user.GetAccountId();
            return Task.FromResult(resource.AccountId == id);
        }
    }
}
