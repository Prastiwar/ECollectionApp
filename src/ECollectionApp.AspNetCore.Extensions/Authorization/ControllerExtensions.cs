using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;

namespace ECollectionApp.AspNetCore.Microservice
{
    public static class ControllerExtensions
    {
        /// <summary> Returns results if user is authenticated to operate on resource, return Forbidden otherwise </summary>
        public static async Task<ActionResult<T>> AuthorizeResultAsync<T>(this ActionResult<T> result, ControllerBase controller, OperationAuthorizationRequirement operation)
        {
            AuthorizationResult authResult = await controller.AuthorizeAsync(result.Value, operation);
            if (authResult.Succeeded)
            {
                return result;
            }
            return controller.Forbid();
        }

        /// <summary> Returns results if user is authenticated to operate on resource, return Forbidden otherwise </summary>
        public static async Task<ActionResult<T>> AuthorizeResultAsync<T>(this ControllerBase controller, T resource, OperationAuthorizationRequirement operation)
        {
            AuthorizationResult authResult = await controller.AuthorizeAsync(resource, operation);
            if (authResult.Succeeded)
            {
                return resource;
            }
            return controller.Forbid();
        }

        /// <summary> Returns results if user is authenticated to operate on resource </summary>
        public static Task<AuthorizationResult> AuthorizeAsync(this ControllerBase controller, object resource, OperationAuthorizationRequirement operation)
        {
            IAuthorizationService service = controller.HttpContext.RequestServices.GetRequiredService<IAuthorizationService>();
            return service.AuthorizeAsync(controller.User, resource, operation);
        }
    }
}
