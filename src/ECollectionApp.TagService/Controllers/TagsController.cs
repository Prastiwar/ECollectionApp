using ECollectionApp.AspNetCore.Microservice;
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
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TagsController : EntityController<Tag>
    {
        public TagsController(TagDbContext context, ILogger<TagsController> logger) : base(context, logger) => context.Database.EnsureCreated();

        protected override int GetEntityId(Tag entity) => entity.Id;

        protected override void SetEntityId(Tag entity, int id) => entity.Id = id;

        // GET: api/tags
        [HttpGet]
        public Task<ActionResult<IEnumerable<Tag>>> GetTag(string search = null) => GetEntities(query => {
            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(t => EF.Functions.Like(t.Name, search));
            }
            return query;
        });

        // GET: api/tags/5
        [HttpGet("{id}")]
        public Task<ActionResult<Tag>> GetTag(int id) => GetEntity(id);

        // PUT: api/tags/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public Task<IActionResult> PutTag(int id, Tag tag) => PutEntity(id, tag);

        // POST: api/tags
        [HttpPost]
        public Task<ActionResult<Tag>> PostTag(Tag tag) => PostEntity(tag, () => CreatedAtAction(nameof(GetTag), new { id = tag.Id }, tag));

        // DELETE: api/tags/5
        [HttpDelete("{id}")]
        public Task<IActionResult> DeleteTag(int id) => DeleteEntity(id);
    }
}
