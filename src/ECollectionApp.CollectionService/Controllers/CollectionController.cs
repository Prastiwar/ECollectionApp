using ECollectionApp.AccountService.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace ECollectionApp.CollectionService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CollectionController : ControllerBase
    {
        public CollectionController(ILogger<CollectionController> logger) => _logger = logger;

        private readonly ILogger<CollectionController> _logger;

        [HttpGet]
        public async Task<IActionResult> Get() => Ok(Array.Empty<Collection>());
    }
}
