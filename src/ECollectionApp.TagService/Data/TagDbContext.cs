using Microsoft.EntityFrameworkCore;

namespace ECollectionApp.TagService.Data
{
    public class TagDbContext : DbContext
    {
        public TagDbContext(DbContextOptions<TagDbContext> options) : base(options) { }

        public DbSet<Tag> Tags { get; set; }

        public DbSet<CollectionGroupTag> CollectionGroupTags { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Tag>();

            modelBuilder.Entity<CollectionGroupTag>()
                        .HasKey(groupTag => new { groupTag.TagId, groupTag.GroupId });
        }
    }
}
