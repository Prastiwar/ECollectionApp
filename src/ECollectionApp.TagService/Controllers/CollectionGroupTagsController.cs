using ECollectionApp.AspNetCore.Microservice;
using ECollectionApp.AspNetCore.Microservice.Authorization;
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
    [Route("api/collection-groups")]
    [ApiController]
    [Authorize]
    public class CollectionGroupTagsController : EntityController<Tag>
    {
        public CollectionGroupTagsController(TagDbContext context, ILogger<CollectionGroupTagsController> logger) : base(context, logger) => context.Database.EnsureCreated();

        protected override int GetEntityId(Tag entity) => entity.Id;

        protected override void SetEntityId(Tag entity, int id) => entity.Id = id;

        // GET: api/collection-groups/5/tags
        [HttpGet("{id}/tags")]
        public async Task<ActionResult<IEnumerable<Tag>>> GetTag(int id, string search = null)
        {
            if (id <= 0)
            {
                return BadRequest();
            }
            IQueryable<CollectionGroupTag> query = from groupTag in Context.Set<CollectionGroupTag>()
                                                   where groupTag.GroupId == id
                                                   select groupTag;
            IQueryable<Tag> tags = from tag in Context.Set<Tag>()
                                   join groupTag in query
                                     on tag.Id equals groupTag.TagId
                                   select tag;
            if (!string.IsNullOrEmpty(search))
            {
                tags = tags.Where(t => EF.Functions.Like(t.Name, search));
            }
            Tag[] result = await tags.ToArrayAsync();
            foreach (Tag tag in result)
            {
                AuthorizationResult authResult = await this.AuthorizeAsync(tag, Operations.Read);
                if (!authResult.Succeeded)
                {
                    return Forbid();
                }
            }
            return result;
        }

        // PUT: api/collection-groups/5/tags
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}/tags")]
        public async Task<IActionResult> PutTag(int id, Tag[] tags)
        {
            if (id == 0)
            {
                return BadRequest();
            }
            // Check if ids are concise and update name's if needed
            for (int i = 0; i < tags.Length; i++)
            {
                Tag tag = tags[i];
                if (tag.Id != 0)
                {
                    Tag foundTag = await Context.Set<Tag>().FindAsync(tag.Id);
                    if (foundTag == null)
                    {
                        return BadRequest();
                    }
                    bool hasDifferentNames = !string.IsNullOrEmpty(tag.Name) && !EqualityComparer<string>.Default.Equals(foundTag.Name, tag.Name);
                    if (hasDifferentNames)
                    {
                        return BadRequest();
                    }
                    tags[i] = foundTag;
                }
            }

            // Update ids or add new tag
            bool needToSave = false;
            for (int i = 0; i < tags.Length; i++)
            {
                Tag tag = tags[i];
                if (tag.Id == 0)
                {
                    Tag foundTag = await Context.Set<Tag>().FirstOrDefaultAsync(t => t.Name == tag.Name);
                    if (foundTag == null)
                    {
                        AuthorizationResult authResult = await this.AuthorizeAsync(tag, Operations.Create);
                        if (!authResult.Succeeded)
                        {
                            return Forbid();
                        }
                        await Context.Set<Tag>().AddAsync(tag);
                        needToSave = true;
                        continue;
                    }
                    tags[i] = foundTag;
                }
            }
            if (needToSave)
            {
                await Context.SaveChangesAsync();
            }
            if (tags.Any(t => t.Id == 0))
            {
                return InternalError();
            }

            IQueryable<CollectionGroupTag> query = from groupTag in Context.Set<CollectionGroupTag>()
                                                   where groupTag.GroupId == id
                                                   select groupTag;
            CollectionGroupTag[] tagsToRemove = await query.ToArrayAsync();
            CollectionGroupTag[] tagsToAdd = tags.Select(t => new CollectionGroupTag() { GroupId = id, TagId = t.Id }).ToArray();
            CollectionGroupTag[] tagsToCompleteRemove = tagsToRemove.Except(tagsToAdd).ToArray();
            CollectionGroupTag[] tagsNotRemoved = tagsToRemove.Except(tagsToCompleteRemove).ToArray();
            if (tagsToCompleteRemove.Length > 0)
            {
                foreach (CollectionGroupTag tag in tagsToCompleteRemove)
                {
                    AuthorizationResult authResult = await this.AuthorizeAsync(tag, Operations.Delete);
                    if (!authResult.Succeeded)
                    {
                        return Forbid();
                    }
                }
                Context.Set<CollectionGroupTag>().RemoveRange(tagsToCompleteRemove);
            }
            CollectionGroupTag[] distinctTagsToAdd = tagsToAdd.Except(tagsNotRemoved).ToArray();
            if (distinctTagsToAdd.Length > 0)
            {
                foreach (CollectionGroupTag tag in distinctTagsToAdd)
                {
                    AuthorizationResult authResult = await this.AuthorizeAsync(tag, Operations.Create);
                    if (!authResult.Succeeded)
                    {
                        return Forbid();
                    }
                }
                await Context.Set<CollectionGroupTag>().AddRangeAsync(distinctTagsToAdd);
                await Context.SaveChangesAsync();
            }
            return NoContent();
        }
    }
}
