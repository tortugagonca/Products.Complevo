using FluentAssertions;
using Products.Complevo.Application.Core.Services.Decorators;
using Products.Complevo.Unit.Tests.Builder;
using System.Linq;
using Xunit;

namespace Products.Complevo.Unit.Tests.Application.Services.Decorators
{
    public class FilterProductByNameTests
    {
        [Theory]
        [InlineData("test")]
        [InlineData("fruit")]
        [InlineData("juice")]
        public void IfExistsProductsByName_MustReturn(string name)
        {
            var products = new ProductBuilder()
                .WithName(name)
                .Build(15);
            var dto = new ProductDtoBuilder()
                .WithName(name)
                .Build();
            var filter = new FilterProductByName(default, dto);
            var queryFiltered = filter.Filter(products.AsQueryable());

            queryFiltered.Should().HaveCount(products.Count());
            queryFiltered.All(x => x.Name.Should().Equals(name));
        }

        [Theory]
        [InlineData("test")]
        [InlineData("fruit")]
        [InlineData("juice")]
        public void IfDoesntExistsProductsByName_MustReturn(string name)
        {
            var products = new ProductBuilder() 
                .Build(15);
            var dto = new ProductDtoBuilder()
                .WithName(name)
                .Build();
            var filter = new FilterProductByName(default, dto);
            var queryFiltered = filter.Filter(products.AsQueryable());

            queryFiltered.Should().BeEmpty(); 
        }
    }
}
