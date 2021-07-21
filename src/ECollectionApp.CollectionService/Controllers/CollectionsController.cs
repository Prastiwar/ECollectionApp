using ECollectionApp.AspNetCore.Microservice;
using ECollectionApp.AspNetCore.Microservice.Authorization;
using ECollectionApp.AspNetCore.Patch;
using ECollectionApp.CollectionService.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECollectionApp.CollectionService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CollectionsController : EntityController<Collection>
    {
        public CollectionsController(CollectionDbContext context, IChangePatcher changePatcher, ILogger<CollectionsController> logger) : base(context, logger)
        {
            ChangePatcher = changePatcher;
            context.Database.EnsureCreated();
        }

        protected IChangePatcher ChangePatcher { get; }

        protected override int GetEntityId(Collection entity) => entity.Id;

        protected override void SetEntityId(Collection entity, int id) => entity.Id = id;

        // GET: api/Collections
        [HttpGet]
        public Task<ActionResult<IEnumerable<Collection>>> GetCollection(int groupId = 0, string search = null) => GetEntities(query => {
            if (groupId > 0)
            {
                query = query.Where(c => c.GroupId == groupId);
            }
            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(c => EF.Functions.Like(c.Name, search));
            }
            return query;
        });

        // GET: api/Collections/5
        [HttpGet("{id}")]
        public Task<ActionResult<Collection>> GetCollection(int id) => GetEntity(id);

        // PATCH: api/Collections/5
        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchCollection(int id, [FromBody] PatchDocument document)
        {
            Collection collection = await Context.Set<Collection>().FindAsync(id);
            if (collection == null)
            {
                return NotFound();
            }
            AuthorizationResult result = await this.AuthorizeAsync(collection, Operations.Update);
            if (!result.Succeeded)
            {
                return Forbid();
            }
            ChangePatcher.ApplyTo(collection, document, ModelState);
            await Context.SaveChangesAsync();
            return NoContent();
        }

        // PUT: api/Collections/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public Task<IActionResult> PutCollection(int id, Collection collection) => PutEntity(id, collection);

        // POST: api/Collections
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public Task<ActionResult<Collection>> PostCollection(Collection collection) => PostEntity(collection, () => CreatedAtAction(nameof(GetCollection), new { id = collection.Id }, collection));

        // DELETE: api/Collections/5
        [HttpDelete("{id}")]
        public Task<IActionResult> DeleteCollection(int id) => DeleteEntity(id);
    }
}