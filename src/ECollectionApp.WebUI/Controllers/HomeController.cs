using ECollectionApp.WebUI.Clients;
using ECollectionApp.WebUI.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ECollectionApp.WebUI.Controllers
{
    public class HomeController : Controller
    {
        public HomeController(ICollectionClient collectionClient, ILogger<HomeController> logger)
        {
            CollectionClient = collectionClient;
            Logger = logger;
        }

        protected ICollectionClient CollectionClient { get; }

        protected ILogger<HomeController> Logger { get; }

        public Task Login() => HttpContext.ChallengeAsync("Auth0", new AuthenticationProperties() { RedirectUri = Url.Action(nameof(Index)) });

        [Authorize]
        public async Task Logout()
        {
            await HttpContext.SignOutAsync("Auth0", new AuthenticationProperties {
                RedirectUri = Url.Action(nameof(Index))
            });
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }

        public async Task<IActionResult> Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                string token = await HttpContext.GetAccessTokenAsync();
                IEnumerable<CollectionGroup> groups = await CollectionClient.WithToken(token).GetGroupsAsync();
                return View(groups);
            }
            return View();
        }
    }
}
