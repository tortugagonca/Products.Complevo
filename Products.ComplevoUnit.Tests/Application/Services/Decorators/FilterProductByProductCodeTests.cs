using FluentAssertions;
using Products.Complevo.Application.Core.Services.Decorators;
using Products.Complevo.Unit.Tests.Builder;
using System.Linq;
using Xunit;

namespace Products.Complevo.Unit.Tests.Application.Services.Decorators
{
    public class FilterProductByProductCodeTests
    {
        [Theory]
        [InlineData(15)]
        [InlineData(23)]
        [InlineData(99)]
        public void IfExistsProductsByProductCode_MustReturn(uint productCode)
        {
            var products = new ProductBuilder()
                .WithProductCode(productCode)
                .Build(15);
            var dto = new ProductDtoBuilder()
                .WithProductCode(productCode)
                .Build();
            var filter = new FilterProductByProductCode(default, dto);
            var queryFiltered = filter.Filter(products.AsQueryable());

            queryFiltered.Should().HaveCount(products.Count());
            queryFiltered.All(x => x.ProductCode.Should().Equals(productCode));
        }

        [Theory]
        [InlineData(15)]
        [InlineData(23)]
        [InlineData(99)]
        public void IfDoesntExistsProductsByProductCode_MustReturn(uint productCode)
        {
            var products = new ProductBuilder()
                .Build(15);
            var dto = new ProductDtoBuilder()
                .WithProductCode(productCode)
                .Build();
            var filter = new FilterProductByName(default, dto);
            var queryFiltered = filter.Filter(products.AsQueryable());

            queryFiltered.Should().BeEmpty();
        }
    }
}
