using Bogus;
using Products.Complevo.Application.Core.Dto;

namespace Products.Complevo.Unit.Tests.Builder
{
    public class ProductDtoBuilder
    {
        private readonly Faker<ProductDto> _faker;
        public ProductDtoBuilder()
        {
            _faker = new Faker<ProductDto>()
                .RuleFor(x => x.Category, _ => _.Commerce.Product())
                .RuleFor(x => x.Name, _ => _.Commerce.ProductName())
                .RuleFor(x => x.Price, _ => double.Parse(_.Commerce.Price()))
                .RuleFor(x => x.ProductCode, _ => _.Random.UInt());
        }

        public ProductDtoBuilder WithCategory(string category)
        {
            _faker.RuleFor(x => x.Category, category);
            return this;
        }
        public ProductDtoBuilder WithName(string name)
        {
            _faker.RuleFor(x => x.Name, name);
            return this;
        }
        public ProductDtoBuilder WithPrice(double price)
        {
            _faker.RuleFor(x => x.Price, price);
            return this;
        }
        public ProductDtoBuilder WithProductCode(uint productCode)
        {
            _faker.RuleFor(x => x.ProductCode, productCode);
            return this;
        }

        public ProductDto Build() => _faker.Generate();
    }
}
