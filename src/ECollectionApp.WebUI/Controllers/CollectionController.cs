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
    public class CollectionController : Controller
    {
        public CollectionController(ICollectionClient collectionClient, ILogger<CollectionGroupController> logger)
        {
            CollectionClient = collectionClient;
            Logger = logger;
        }

        protected ICollectionClient CollectionClient { get; }

        protected ILogger<CollectionGroupController> Logger { get; }

        /// <summary> Return CollectionClient with access token </summary>
        protected async Task<ICollectionClient> GetCollectionClientAsync() => CollectionClient.WithToken(await HttpContext.GetAccessTokenAsync());

        // GET: Collection/Move
        public async Task<IActionResult> Move(Collection collection)
        {
            if (collection.Id == 0 || collection.GroupId == 0)
            {
                return BadRequest();
            }
            ICollectionClient client = await GetCollectionClientAsync();
            IEnumerable<CollectionGroup> groups = await client.GetGroupsAsync();
            CollectionMoveViewModel viewModel = new CollectionMoveViewModel() {
                Collection = collection,
                Groups = groups
            };
            return View(viewModel);
        }

        // POST: Collection/Move
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Move(Collection collection, int groupId, bool showGroup = false)
        {
            if (collection.GroupId == 0 || groupId == 0)
            {
                return BadRequest();
            }
            ICollectionClient client = await GetCollectionClientAsync();
            try
            {
                int previousGroupId = collection.GroupId;
                collection.GroupId = groupId;
                await client.UpdateCollectionAsync(collection);
                return RedirectToAction("Details", "CollectionGroup", new { id = showGroup ? groupId : previousGroupId });
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, $"Couldn't move collection with id: {collection.Id} to group of id: {groupId}");
                return View(collection);
            }
        }

        // GET: Collection/Create?groupId=2
        public IActionResult Create([FromQuery] int groupId)
        {
            if (groupId == 0)
            {
                return BadRequest();
            }
            Collection collection = new Collection() {
                GroupId = groupId,
                Name = "New collection",
                Value = ""
            };
            return View(collection);
        }

        // POST: Collection/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Collection collection)
        {
            ICollectionClient client = await GetCollectionClientAsync();
            try
            {
                await client.CreateCollectionAsync(collection);
                return RedirectToAction("Details", "CollectionGroup", new { id = collection.GroupId });
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Couldn't create group with id " + collection.Id);
                return View(collection);
            }
        }

        // GET: Collection/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            ICollectionClient client = await GetCollectionClientAsync();
            Collection collection = await client.GetCollectionAsync(id);
            if (collection == null)
            {
                return NotFound();
            }
            return View(collection);
        }

        // POST: Collection/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Collection collection)
        {
            if (id != collection.Id || collection.GroupId == 0)
            {
                return BadRequest();
            }
            ICollectionClient client = await GetCollectionClientAsync();
            try
            {
                await client.UpdateCollectionAsync(collection);
                return RedirectToAction("Details", "CollectionGroup", new { id = collection.GroupId });
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Couldn't update group with id " + id);
                return View(collection);
            }
        }

        // GET: Collection/Delete/5?groupId=2
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id, [FromQuery] int groupId)
        {
            ICollectionClient client = await GetCollectionClientAsync();
            try
            {
                await client.DeleteCollectionAsync(id);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Couldn't remove group with id " + id);
            }
            return RedirectToAction("Details", "CollectionGroup", new { id = groupId });
        }
    }
}
