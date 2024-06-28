using Microsoft.EntityFrameworkCore;
using TestProj.Entities;

namespace TestProj.Persistence
{
    public class SupplierAppDbContext(DbContextOptions options) : DbContext(options)

    {
        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>(eb =>
            {
                eb.HasKey(p => p.Id);
                eb.Property(p => p.Id).IsRequired();
                eb.Property(p => p.Price).HasPrecision(7, 2);
            });
        }
    }
}
