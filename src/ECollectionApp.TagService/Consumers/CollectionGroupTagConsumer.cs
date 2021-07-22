using ECollectionApp.CollectionGroupService.Messaging;
using ECollectionApp.TagService.Data;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace ECollectionApp.TagService.Consumers
{
    public class CollectionGroupTagConsumer : IConsumer<CollectionGroupDeletedEvent>
    {
        public CollectionGroupTagConsumer(TagDbContext context) => Context = context;

        protected TagDbContext Context { get; }

        public async Task Consume(ConsumeContext<CollectionGroupDeletedEvent> context)
        {
            int id = context.Message.GroupId;
            IQueryable<CollectionGroupTag> query = from groupTag in Context.Set<CollectionGroupTag>()
                                                   where groupTag.GroupId == id
                                                   select groupTag;
            CollectionGroupTag[] tags = await query.ToArrayAsync();
            Context.RemoveRange(tags);
            await Context.SaveChangesAsync();
        }
    }
}
