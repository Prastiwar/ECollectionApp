using ECollectionApp.CollectionService.Data;
using ECollectionApp.CollectionService.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECollectionApp.CollectionService.Controllers
{
    [Route("api/collection-groups")]
    [ApiController]
    [Authorize]
    public class CollectionGroupsController : ControllerBase
    {
        public CollectionGroupsController(CollectionGroupDbContext context, ILogger<CollectionGroupsController> logger)
        {
            Context = context;
            Logger = logger;
        }

        protected CollectionGroupDbContext Context { get; }

        protected ILogger<CollectionGroupsController> Logger { get; }

        // GET: api/collection-groups
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CollectionGroup>>> GetCollectionGroup() => await Context.CollectionGroup.ToListAsync();

        // GET: api/collection-groups/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CollectionGroup>> GetCollectionGroup(int id)
        {
            CollectionGroup collectionGroup = await Context.CollectionGroup.FindAsync(id);
            if (collectionGroup == null)
            {
                return NotFound();
            }
            return collectionGroup;
        }

        // PUT: api/collection-groups/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCollectionGroup(int id, CollectionGroup collectionGroup)
        {
            if (id != collectionGroup.Id)
            {
                return BadRequest();
            }
            Context.Entry(collectionGroup).State = EntityState.Modified;
            try
            {
                await Context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!CollectionGroupExists(id))
                {
                    return NotFound();
                }
                Logger.LogError(ex, $"Error occured while executing {nameof(PutCollectionGroup)}");
                throw;
            }
            return NoContent();
        }

        // POST: api/collection-groups
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<CollectionGroup>> PostCollectionGroup(CollectionGroup collectionGroup)
        {
            Context.CollectionGroup.Add(collectionGroup);
            await Context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetCollectionGroup), new { id = collectionGroup.Id }, collectionGroup);
        }

        // DELETE: api/collection-groups/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCollectionGroup(int id)
        {
            CollectionGroup collectionGroup = await Context.CollectionGroup.FindAsync(id);
            if (collectionGroup == null)
            {
                return NotFound();
            }
            Context.CollectionGroup.Remove(collectionGroup);
            await Context.SaveChangesAsync();
            return NoContent();
        }

        private bool CollectionGroupExists(int id) => Context.CollectionGroup.Any(e => e.Id == id);
    }
}
