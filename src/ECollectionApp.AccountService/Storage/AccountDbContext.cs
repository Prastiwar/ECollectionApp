using ECollectionApp.AccountService.Models;
using Microsoft.EntityFrameworkCore;

namespace ECollectionApp.AccountService.Storage
{
    public class AccountDbContext : DbContext
    {
        public AccountDbContext(DbContextOptions options) : base(options) { }

        public DbSet<Account> Accounts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Account>();
        }
    }
}
