using Products.Complevo.Domain.Models;
using System.Threading.Tasks;

namespace Products.Complevo.Application.Core.Interfaces.Repository
{
    public interface IProductRepository : IBaseRepository<Product>
    { 
        Task<Product> GetProductByNameAndCodeAsync(string name, uint productCode);
    }
}