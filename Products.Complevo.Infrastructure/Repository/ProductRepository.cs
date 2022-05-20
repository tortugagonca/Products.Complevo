using Microsoft.EntityFrameworkCore;
using Products.Complevo.Application.Core.Interfaces.Repository;
using Products.Complevo.Application.Core.Services.Decorators;
using Products.Complevo.Domain.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Products.Complevo.Infrastructure.Data.Repository
{
    public class ProductRepository : BaseRepository, IProductRepository
    {
        private readonly IFilterProductsFactory _filterProductsFactory;
        public ProductRepository(ProductsComplevoContext context, 
            IFilterProductsFactory filterProductsFactory) 
            : base(context)
        {
            this._filterProductsFactory = filterProductsFactory;
        }

        public async Task<Product> GetByIdAsync(Guid id)
            => await _context.Products
            .AsNoTracking()
            .Where(x => x.Id == id)
            .FirstOrDefaultAsync();

        public async Task<Product> GetByIdWithTrackingAsync(Guid id)
            => await _context.Products 
            .Where(x => x.Id == id)
            .FirstOrDefaultAsync();

        public async Task<Product> InsertAsync(Product entity)
            => (await _context
                        .Products
                        .AddAsync(entity))
                        .Entity;

        public async Task<Product> GetProductByNameAndCodeAsync(string name, uint productCode)
        {
            var products = _context.Products;
            var filteredProducts = await _filterProductsFactory
                .GetProductsAsync(new() { Name = name, ProductCode = productCode }, products);
            return filteredProducts
                .FirstOrDefault();
        } 

        public async Task<int> DeleteAsync(Product entity)
        {
            _context.Products.Remove(entity);
            return await _context.SaveChangesAsync();
        }
    }
}
