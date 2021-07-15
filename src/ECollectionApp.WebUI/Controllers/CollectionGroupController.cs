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

        // GET: CollectionGroupController
        public async Task<IActionResult> Index()
        {
            string token = await HttpContext.GetAccessTokenAsync();
            IEnumerable<CollectionGroup> groups = await CollectionClient.WithToken(token).GetGroupsAsync();
            return View(groups);
        }

        // GET: CollectionGroupController/Details/5
        public async Task<IActionResult> Details(int id)
        {
            CollectionGroup group = await CollectionClient.GetGroupAsync(id);
            if (group == null)
            {
                return NotFound();
            }
            CollectionGroupViewModel viewModel = new CollectionGroupViewModel() {
                Group = group,
                Collections = await CollectionClient.GetCollectionsAsync(group.Id)
            };
            return View(viewModel);
        }

        // GET: CollectionGroupController/Create
        public IActionResult Create()
        {
            CollectionGroup group = new CollectionGroup() {
                AccountId = int.Parse(User.FindFirst("").Value),
                Name = "New group"
            };
            return View(group);
        }

        // POST: CollectionGroupController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CollectionGroup group)
        {
            try
            {
                await CollectionClient.CreateGroupAsync(group);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: CollectionGroupController/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            CollectionGroup group = await CollectionClient.GetGroupAsync(id);
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
            try
            {
                await CollectionClient.UpdateGroupAsync(group);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: CollectionGroupController/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await CollectionClient.DeleteGroupAsync(id);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Couldn't remove group with id " + id);
            }
            return View();
        }
    }
}
