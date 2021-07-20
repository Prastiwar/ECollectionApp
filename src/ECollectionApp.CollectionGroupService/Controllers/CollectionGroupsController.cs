using ECollectionApp.AspNetCore.Microservice;
using ECollectionApp.CollectionGroupService.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECollectionApp.CollectionGroupService.Controllers
{
    [Route("api/collection-groups")]
    [ApiController]
    [Authorize]
    public class CollectionGroupsController : EntityController<CollectionGroup>
    {
        public CollectionGroupsController(CollectionGroupDbContext context, ILogger<CollectionGroupsController> logger) : base(context, logger) => context.Database.EnsureCreated();

        protected override int GetEntityId(CollectionGroup entity) => entity.Id;

        protected override void SetEntityId(CollectionGroup entity, int id) => entity.Id = id;

        // GET: api/collection-groups
        [HttpGet]
        public Task<ActionResult<IEnumerable<CollectionGroup>>> GetCollectionGroups(string search = null) => GetEntities(query => {
            int accountId = User.GetAccountId();
            query = query.Where(g => g.AccountId == accountId);
            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(g => EF.Functions.Like(g.Name, search));
            }
            return query;
        });

        // GET: api/collection-groups/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CollectionGroup>> GetCollectionGroup(int id)
        {
            ActionResult<CollectionGroup> result = await GetEntity(id);
            int accountId = User.GetAccountId();
            if (result.Value != null && result.Value.AccountId != accountId)
            {
                return Unauthorized();
            }
            return result;
        }

        // PUT: api/collection-groups/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCollectionGroup(int id, CollectionGroup collectionGroup)
        {
            int entityId = GetEntityId(collectionGroup);
            if (collectionGroup == null || (entityId > 0 && entityId != id))
            {
                return BadRequest();
            }
            CollectionGroup foundGroup = await Context.Set<CollectionGroup>().FindAsync(id);
            if (foundGroup == null)
            {
                return NotFound();
            }
            if (collectionGroup.AccountId != foundGroup.AccountId)
            {
                return BadRequest();
            }
            int accountId = User.GetAccountId();
            if (collectionGroup.AccountId != accountId)
            {
                return Unauthorized();
            }
            foundGroup.Name = collectionGroup.Name;
            await Context.SaveChangesAsync();
            return NoContent();
        }

        // POST: api/collection-groups
        [HttpPost]
        public Task<ActionResult<CollectionGroup>> PostCollectionGroup(CollectionGroup collectionGroup)
        {
            int accountId = User.GetAccountId();
            if (collectionGroup.AccountId != accountId)
            {
                return Task.FromResult<ActionResult<CollectionGroup>>(Unauthorized());
            }
            return PostEntity(collectionGroup, () => CreatedAtAction(nameof(GetCollectionGroup), new { id = collectionGroup.Id }, collectionGroup));
        }

        // DELETE: api/collection-groups/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCollectionGroup(int id)
        {
            CollectionGroup collectionGroup = await Context.Set<CollectionGroup>().FindAsync(id);
            if (collectionGroup == null)
            {
                return NotFound();
            }
            int accountId = User.GetAccountId();
            if (collectionGroup.AccountId != accountId)
            {
                return Unauthorized();
            }
            Context.Set<CollectionGroup>().Remove(collectionGroup);
            await Context.SaveChangesAsync();
            return NoContent();
        }
    }
}
