using Products.Complevo.Application.Core.Dto;
using Products.Complevo.Domain.Models;
using System.Linq;

namespace Products.Complevo.Application.Core.Services.Decorators
{
    public interface IFilterDecorator
    {
        IFilterDecorator NextDecorator { get; set; }
        ProductDto FilterDto { get; set; }
        IQueryable<Product> Filter(IQueryable<Product> query);
    }
}