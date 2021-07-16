using ECollectionApp.WebUI.Clients;
using ECollectionApp.WebUI.Data;
using ECollectionApp.WebUI.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ECollectionApp.WebUI.Controllers
{
    [Authorize]
    public class CollectionGroupController : Controller
    {
        public CollectionGroupController(ICollectionClient collectionClient, ILogger<CollectionGroupController> logger)
        {
            CollectionClient = collectionClient;
            Logger = logger;
        }

        protected ICollectionClient CollectionClient { get; }

        protected ILogger<CollectionGroupController> Logger { get; }

        /// <summary> Return CollectionClient with access token </summary>
        protected async Task<ICollectionClient> GetCollectionClientAsync() => CollectionClient.WithToken(await HttpContext.GetAccessTokenAsync());

        // GET: CollectionGroupController
        public async Task<IActionResult> Index()
        {
            ICollectionClient client = await GetCollectionClientAsync();
            IEnumerable<CollectionGroup> groups = await client.GetGroupsAsync(User.GetAccountId());
            return View(groups);
        }

        // GET: CollectionGroupController/Details/5
        public async Task<IActionResult> Details(int id)
        {
            ICollectionClient client = await GetCollectionClientAsync();
            CollectionGroup group = await client.GetGroupAsync(id);
            if (group == null)
            {
                return NotFound();
            }
            CollectionGroupViewModel viewModel = new CollectionGroupViewModel() {
                Group = group,
                Collections = await client.GetCollectionsAsync(group.Id)
            };
            return View(viewModel);
        }

        // GET: CollectionGroupController/Create
        public IActionResult Create()
        {
            CollectionGroup group = new CollectionGroup() {
                AccountId = User.GetAccountId(),
                Name = "New group"
            };
            return View(group);
        }

        // POST: CollectionGroupController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CollectionGroup group)
        {
            ICollectionClient client = await GetCollectionClientAsync();
            try
            {
                await client.CreateGroupAsync(group);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Couldn't create group with id " + group.Id);
                return View(group);
            }
        }

        // GET: CollectionGroupController/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            ICollectionClient client = await GetCollectionClientAsync();
            CollectionGroup group = await client.GetGroupAsync(id);
            if (group == null)
            {
                return NotFound();
            }
            return View(group);
        }

        // POST: CollectionGroupController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CollectionGroup group)
        {
            if (id != group.Id)
            {
                return BadRequest();
            }
            ICollectionClient client = await GetCollectionClientAsync();
            try
            {
                await client.UpdateGroupAsync(group);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Couldn't update group with id " + id);
                return View(group);
            }
        }

        // GET: CollectionGroupController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            ICollectionClient client = await GetCollectionClientAsync();
            try
            {
                await client.DeleteGroupAsync(id);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Couldn't remove group with id " + id);
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
