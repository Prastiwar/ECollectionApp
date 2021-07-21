using ECollectionApp.AspNetCore.Microservice.Authorization;
using ECollectionApp.TagService.Data;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ECollectionApp.TagService.Authorization
{
    public class CollectionGroupTagAuthorizationHandler : ResourceAuthorizationHandler<CollectionGroupTag>
    {
        protected override Task<bool> AuthorizeAsync(ClaimsPrincipal user, CollectionGroupTag resource, string operation) => Task.FromResult(true); // TODO: Authorize
    }
}
