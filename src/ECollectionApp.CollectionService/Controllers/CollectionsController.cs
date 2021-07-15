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
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CollectionsController : ControllerBase
    {
        public CollectionsController(CollectionDbContext context, ILogger<CollectionsController> logger)
        {
            Context = context;
            context.Database.EnsureCreated();
            Logger = logger;
        }

        protected CollectionDbContext Context;

        protected ILogger<CollectionsController> Logger;

        // GET: api/Collections
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Collection>>> GetCollection() => await Context.Collections.ToListAsync();

        // GET: api/Collections/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Collection>> GetCollection(int id)
        {
            Collection collection = await Context.Collections.FindAsync(id);
            if (collection == null)
            {
                return NotFound();
            }
            return collection;
        }

        // PUT: api/Collections/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCollection(int id, Collection collection)
        {
            if (id != collection.Id)
            {
                return BadRequest();
            }
            Context.Entry(collection).State = EntityState.Modified;
            try
            {
                await Context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!CollectionExists(id))
                {
                    return NotFound();
                }
                Logger.LogError(ex, $"Error occured while executing {nameof(PutCollection)}");
                throw;
            }
            return NoContent();
        }

        // POST: api/Collections
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Collection>> PostCollection(Collection collection)
        {
            await Context.Collections.AddAsync(collection);
            await Context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetCollection), new { id = collection.Id }, collection);
        }

        // DELETE: api/Collections/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCollection(int id)
        {
            Collection collection = await Context.Collections.FindAsync(id);
            if (collection == null)
            {
                return NotFound();
            }
            Context.Collections.Remove(collection);
            await Context.SaveChangesAsync();
            return NoContent();
        }

        private bool CollectionExists(int id) => Context.Collections.Any(e => e.Id == id);
    }
}
