using Microsoft.EntityFrameworkCore;

namespace ECollectionApp.TagService.Data
{
    public class TagDbContext : DbContext
    {
        public TagDbContext(DbContextOptions<TagDbContext> options) : base(options) { }

        public DbSet<CollectionGroupTag> CollectionGroupTags { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<CollectionGroupTag>();
        }
    }
}
