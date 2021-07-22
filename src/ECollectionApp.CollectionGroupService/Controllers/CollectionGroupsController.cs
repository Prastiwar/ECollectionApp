using ECollectionApp.AspNetCore.Microservice;
using ECollectionApp.CollectionGroupService.Data;
using MassTransit;
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
        public CollectionGroupsController(CollectionGroupDbContext context, IPublishEndpoint publishEndpoint, ILogger<CollectionGroupsController> logger) : base(context, logger)
        {
            context.Database.EnsureCreated();
            PublishEndpoint = publishEndpoint;
        }

        protected IPublishEndpoint PublishEndpoint { get; }

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
        public Task<ActionResult<CollectionGroup>> GetCollectionGroup(int id) => GetEntity(id);

        // PUT: api/collection-groups/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public Task<IActionResult> PutCollectionGroup(int id, CollectionGroup collectionGroup) => PutEntity(id, collectionGroup);

        // POST: api/collection-groups
        [HttpPost]
        public Task<ActionResult<CollectionGroup>> PostCollectionGroup(CollectionGroup collectionGroup)
            => PostEntity(collectionGroup, () => CreatedAtAction(nameof(GetCollectionGroup), new { id = collectionGroup.Id }, collectionGroup));

        // DELETE: api/collection-groups/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCollectionGroup(int id)
        {
            //await PublishEndpoint.Publish<>(,);
            return await DeleteEntity(id);
        }
    }
}
