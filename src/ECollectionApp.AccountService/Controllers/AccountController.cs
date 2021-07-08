using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ECollectionApp.AccountService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccountController : ControllerBase
    {
        public AccountController(ILogger<AccountController> logger) => _logger = logger;

        private readonly ILogger<AccountController> _logger;
    }
}
