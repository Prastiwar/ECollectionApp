using ECollectionApp.CollectionService.Models;
using Microsoft.EntityFrameworkCore;

namespace ECollectionApp.CollectionService
{
    public class CollectionDbContext : DbContext
    {
        public CollectionDbContext(DbContextOptions<CollectionDbContext> options) : base(options) { }

        public DbSet<Collection> Collections { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Collection>();
        }
    }
}
