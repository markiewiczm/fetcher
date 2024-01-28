using Microsoft.EntityFrameworkCore;
using Extractor.Data.Entities;

namespace Extractor.Repository
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Product>().ToTable(nameof(Product), "dbo");
            modelBuilder.Entity<Product>().HasKey(x => new { x.Id });

            base.OnModelCreating(modelBuilder);
        }

    }
}
