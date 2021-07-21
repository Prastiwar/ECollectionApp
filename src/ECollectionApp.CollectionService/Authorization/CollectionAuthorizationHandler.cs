using ECollectionApp.AspNetCore.Microservice.Authorization;
using ECollectionApp.CollectionService.Data;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ECollectionApp.CollectionService.Authorization
{
    public class CollectionAuthorizationHandler : ResourceAuthorizationHandler<Collection>
    {
        protected override Task<bool> AuthorizeAsync(ClaimsPrincipal user, Collection resource, string operation)
        {
            int id = user.GetAccountId();
            int groupId = resource.GroupId;
            // TODO: Authorize
            return Task.FromResult(true);
        }
    }
}
