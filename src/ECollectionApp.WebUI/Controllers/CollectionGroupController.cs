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
        public CollectionGroupController(ICollectionClient collectionClient, ITagClient tagClient, ILogger<CollectionGroupController> logger)
        {
            CollectionClient = collectionClient;
            TagClient = tagClient;
            Logger = logger;
        }

        protected ICollectionClient CollectionClient { get; }
        protected ITagClient TagClient { get; }

        protected ILogger<CollectionGroupController> Logger { get; }

        /// <summary> Return CollectionClient with access token </summary>
        protected async Task<ICollectionClient> GetCollectionClientAsync() => CollectionClient.WithToken(await HttpContext.GetAccessTokenAsync());

        protected async Task<ITagClient> GetTagClientAsync() => TagClient.WithToken(await HttpContext.GetAccessTokenAsync());

        // GET: CollectionGroupController
        public async Task<IActionResult> Index()
        {
            ICollectionClient client = await GetCollectionClientAsync();
            IEnumerable<CollectionGroup> groups = await client.GetGroupsAsync(User.GetAccountId(), true);
            return View(groups);
        }

        // GET: CollectionGroupController/Details/5
        public async Task<IActionResult> Details(int id)
        {
            ICollectionClient client = await GetCollectionClientAsync();
            CollectionGroup group = await client.GetGroupAsync(id, true);
            if (group == null)
            {
                return NotFound();
            }
            CollectionGroupViewModel viewModel = new CollectionGroupViewModel() {
                Group = group,
                Collections = await client.GetCollectionsAsync(group.Id),
                Tags = group.Tags != null ? string.Join(',', group.Tags) : ""
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
            CollectionGroupViewModel viewModel = new CollectionGroupViewModel() {
                Group = group,
                Collections = new List<Collection>(),
                Tags = ""
            };
            return View(viewModel);
        }

        // POST: CollectionGroupController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CollectionGroupViewModel viewModel)
        {
            ICollectionClient client = await GetCollectionClientAsync();
            try
            {
                await client.CreateGroupAsync(viewModel.Group);
                string[] tags = viewModel.Tags?.Trim().Split(',') ?? Array.Empty<string>();
                ITagClient tagClient = await GetTagClientAsync();
                await tagClient.UpdateGroupTagsAsync(viewModel.Group.Id, tags);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Couldn't create group with id " + viewModel.Group.Id);
                return View(viewModel);
            }
        }

        // GET: CollectionGroupController/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            ICollectionClient client = await GetCollectionClientAsync();
            CollectionGroup group = await client.GetGroupAsync(id, true);
            if (group == null)
            {
                return NotFound();
            }
            CollectionGroupViewModel viewModel = new CollectionGroupViewModel() {
                Group = group,
                Collections = new List<Collection>(),
                Tags = ""
            };
            return View(viewModel);
        }

        // POST: CollectionGroupController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CollectionGroupViewModel viewModel)
        {
            if (id != viewModel.Group.Id)
            {
                return BadRequest();
            }
            ICollectionClient client = await GetCollectionClientAsync();
            try
            {
                await client.UpdateGroupAsync(viewModel.Group);
                string[] tags = viewModel.Tags?.Trim().Split(',') ?? Array.Empty<string>();
                ITagClient tagClient = await GetTagClientAsync();
                await tagClient.UpdateGroupTagsAsync(viewModel.Group.Id, tags);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Couldn't update group with id " + id);
                return View(viewModel);
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
