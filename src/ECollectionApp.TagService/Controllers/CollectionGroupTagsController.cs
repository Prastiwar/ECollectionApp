using ECollectionApp.TagService.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECollectionApp.CollectionService.Controllers
{
    [Route("api/tags/collection-groups")]
    [ApiController]
    [Authorize]
    public class CollectionGroupTagsController : ControllerBase
    {
        public CollectionGroupTagsController(TagDbContext context, ILogger<CollectionGroupTagsController> logger)
        {
            Context = context;
            context.Database.EnsureCreated();
            Logger = logger;
        }

        protected TagDbContext Context { get; }

        protected ILogger<CollectionGroupTagsController> Logger { get; }

        // GET: api/tags/collection-groups
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CollectionGroupTag>>> GetCollectionGroupTag() => await Context.CollectionGroupTags.ToListAsync();

        // GET: api/tags/collection-groups/5
        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<CollectionGroupTag>>> GetCollectionGroupTag(int id) => await Context.CollectionGroupTags.Where(t => t.GroupId == id).ToListAsync();

        // PUT: api/tags/collection-groups/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCollectionGroupTag(int id, CollectionGroupTag tag)
        {
            if (id != tag.Id)
            {
                return BadRequest();
            }
            Context.Entry(tag).State = EntityState.Modified;
            try
            {
                await Context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!CollectionGroupTagExists(id))
                {
                    return NotFound();
                }
                Logger.LogError(ex, $"Error occured while executing {nameof(PutCollectionGroupTag)}");
                throw;
            }
            return NoContent();
        }

        // POST: api/tags/collection-groups
        [HttpPost]
        public async Task<ActionResult<CollectionGroupTag>> PostCollectionGroupTag(CollectionGroupTag tag)
        {
            Context.CollectionGroupTags.Add(tag);
            await Context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetCollectionGroupTag), new { id = tag.Id }, tag);
        }

        // DELETE: api/tags/collection-groups/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCollectionGroupTag(int id)
        {
            CollectionGroupTag tag = await Context.CollectionGroupTags.FindAsync(id);
            if (tag == null)
            {
                return NotFound();
            }
            Context.CollectionGroupTags.Remove(tag);
            await Context.SaveChangesAsync();
            return NoContent();
        }

        private bool CollectionGroupTagExists(int id) => Context.CollectionGroupTags.Any(e => e.Id == id);
    }
}
