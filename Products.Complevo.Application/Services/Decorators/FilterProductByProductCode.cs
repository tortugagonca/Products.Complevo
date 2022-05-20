using Products.Complevo.Application.Core.Dto;
using Products.Complevo.Domain.Models;
using System.Linq;

namespace Products.Complevo.Application.Core.Services.Decorators
{
    public class FilterProductByProductCode : BaseFilter
    {
        public FilterProductByProductCode(IFilterDecorator nextDecorator, ProductDto filter) : base(nextDecorator, filter)
        {
        }

        public override IQueryable<Product> Filter(IQueryable<Product> query)
        {
            var filteredQuery = NextDecorator?.Filter(query) ?? query;
            return filteredQuery.Where(x =>  x.ProductCode == FilterDto.ProductCode);
        }
    }
}
