using Products.Complevo.Application.Core.Dto;
using Products.Complevo.Domain.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Products.Complevo.Application.Core.Services.Decorators
{
    public interface IFilterProductsFactory
    {
        Task<IEnumerable<Product>> GetProductsAsync(ProductDto productDto, IQueryable<Product> products);
    }
}
