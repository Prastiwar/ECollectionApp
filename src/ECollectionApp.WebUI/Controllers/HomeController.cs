using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace ECollectionApp.WebUI.Controllers
{
    public class HomeController : Controller
    {
        public HomeController(ILogger<HomeController> logger) => Logger = logger;

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

        public IActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction(nameof(Index), nameof(CollectionGroupController).Replace("Controller", ""));
            }
#if DEBUG
            return RedirectToAction(nameof(Login));
#else
            return View();
#endif
        }
    }
}
