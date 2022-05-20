using Microsoft.EntityFrameworkCore;
using Products.Complevo.Application.Core.Dto;
using Products.Complevo.Application.Core.Services.Decorators;
using Products.Complevo.Domain.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Products.Complevo.Infrastructure.Data.Repository.Decorators
{
    public class FilterProductsFactory : IFilterProductsFactory
    {
        private IFilterDecorator Create(ProductDto filter)
        {
            IFilterDecorator decorator = default;
            decorator = new FilterProductByName(decorator, filter);
            decorator = new FilterProductByProductCode(decorator, filter);
            return decorator;
        }

        public async Task<IEnumerable<Product>> GetProductsAsync(ProductDto productDto, IQueryable<Product> products)
         => await Create(productDto)
                .Filter(products)
                .ToListAsync();
    }
}
