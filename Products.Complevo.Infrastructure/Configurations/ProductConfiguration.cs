using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Products.Complevo.Domain.Models;
using System.Diagnostics.CodeAnalysis;

namespace Products.Complevo.Infrastructure.Data.Configurations
{
    [ExcludeFromCodeCoverage]
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable(nameof(Product));
            builder.HasKey(c => c.Id); 
            builder.Property(c => c.Name).HasMaxLength(200).IsRequired();
            builder.Property(c => c.ProductCode).IsRequired();
            builder.Property(c => c.Price).IsRequired();
            builder.HasIndex(c => new { c.ProductCode, c.Name }).IsUnique(true).HasDatabaseName("Product-Unicity");
        }
    }
}
