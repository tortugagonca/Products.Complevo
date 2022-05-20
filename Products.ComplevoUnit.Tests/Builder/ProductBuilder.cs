using Bogus;
using Products.Complevo.Domain.Models;
using System;
using System.Collections.Generic;

namespace Products.Complevo.Unit.Tests.Builder
{
    public class ProductBuilder
    {
        private readonly Faker<Product> _faker;
        public ProductBuilder()
        {
            _faker = new Faker<Product>()
                .RuleFor(x => x.Id, _ => _.Random.Guid())
                .RuleFor(x => x.Category, _ => _.Commerce.Product())
                .RuleFor(x => x.Name, _ => _.Commerce.ProductName())
                .RuleFor(x => x.Price, _ => double.Parse(_.Commerce.Price()))
                .RuleFor(x => x.ProductCode, _ => _.Random.UInt());
        }

        public ProductBuilder WithCategory(string category)
        {
            _faker.RuleFor(x => x.Category, category);
            return this;
        }

        public ProductBuilder WithName(string name)
        {
            _faker.RuleFor(x => x.Name, name);
            return this;
        }

        public ProductBuilder WithPrice(double price)
        {
            _faker.RuleFor(x => x.Price, price);
            return this;
        }

        public ProductBuilder WithId(Guid id)
        {
            _faker.RuleFor(x => x.Id, id);
            return this;
        }

        public ProductBuilder WithProductCode(uint productCode)
        {
            _faker.RuleFor(x => x.ProductCode, productCode);
            return this;
        }

        public Product Build() => _faker.Generate();
        public IEnumerable<Product> Build(int count = 10) => _faker.Generate(count);
    }
}
