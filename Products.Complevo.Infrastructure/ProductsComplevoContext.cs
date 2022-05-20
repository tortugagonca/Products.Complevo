using Microsoft.EntityFrameworkCore;
using Products.Complevo.Domain.Models;
using Products.Complevo.Infrastructure.Data.Configurations;

namespace Products.Complevo.Infrastructure.Data
{
    public class ProductsComplevoContext : DbContext
    {
        public DbSet<Product> Products { get; set; }

        public ProductsComplevoContext(DbContextOptions options):base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfiguration(new ProductConfiguration()); 
        }
    }
}
