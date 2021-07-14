﻿using ECollectionApp.CollectionService.Models;
using Microsoft.EntityFrameworkCore;

namespace ECollectionApp.CollectionService.Data
{
    public class CollectionGroupDbContext : DbContext
    {
        public CollectionGroupDbContext(DbContextOptions<CollectionGroupDbContext> options) : base(options) { }

        public DbSet<CollectionGroup> CollectionGroup { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<CollectionGroup>();
        }
    }
}
