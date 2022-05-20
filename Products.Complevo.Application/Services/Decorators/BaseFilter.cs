using Products.Complevo.Application.Core.Dto;
using Products.Complevo.Domain.Models;
using System.Linq;

namespace Products.Complevo.Application.Core.Services.Decorators
{
    public abstract class BaseFilter : IFilterDecorator
    {
        public IFilterDecorator NextDecorator { get; set; }
        public ProductDto FilterDto { get; set; }

        protected BaseFilter(
            IFilterDecorator nextDecorator,
            ProductDto filter
        )
        {
            this.NextDecorator = nextDecorator;
            this.FilterDto = filter;
        }

        public abstract IQueryable<Product> Filter(IQueryable<Product> query);
    }
}
