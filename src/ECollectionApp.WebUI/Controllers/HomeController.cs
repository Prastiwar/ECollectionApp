using ECollectionApp.WebUI.Clients;
using ECollectionApp.WebUI.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace ECollectionApp.WebUI.Controllers
{
    public class HomeController : Controller
    {
        public HomeController(ICollectionClient collectionClient, IAccountClient accountClient, ILogger<HomeController> logger)
        {
            CollectionClient = collectionClient;
            AccountClient = accountClient;
            Logger = logger;
        }

        protected ICollectionClient CollectionClient { get; }

        protected IAccountClient AccountClient { get; }

        protected ILogger<HomeController> Logger { get; }

        public async Task<IActionResult> Login()
        {
            if (!User.Identity.IsAuthenticated)
            {
                string token = await AccountClient.Login();
            }
            return View("Index");
        }

        public async Task<IActionResult> Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                string token = await HttpContext.GetTokenAsync(JwtBearerDefaults.AuthenticationScheme);
                string token2 = User.Identity.Name;
                //HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, Api.Collection.GetGroups());
                //request.Headers.Add(JwtBearerDefaults.AuthenticationScheme, token);
                IEnumerable<CollectionGroup> groups = await CollectionClient.GetGroupsAsync();
                return View("CollectionGroups", groups);
            }
            return View();
        }
    }
}
