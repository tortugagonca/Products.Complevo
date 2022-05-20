using FluentAssertions;
using Moq;
using Products.Complevo.Application.Core.Interfaces.Repository;
using Products.Complevo.Service.Commands.Products;
using Products.Complevo.Service.Commands.Validator;
using Products.Complevo.Unit.Tests.Builder;
using System;
using System.Linq;
using Xunit;

namespace Products.Complevo.Unit.Tests.Services.Commands.Validators
{
    [Trait("Unity", nameof(UpdateProductCommandValidator))]
    public class UpdateProductCommandValidatorTests
    { 
        [Fact]
        public void IfNameEmpty_MustReturnError()
        {
            var id = Guid.NewGuid();
            var validator = new ValidatorBuilder()
                .WithProductPersisted(id)
                .Build();

            var dto = new ProductDtoBuilder() 
                .WithName(string.Empty)
                .Build();

            var comando = new UpdateProductCommand(
                id,
                dto.ProductCode,
                dto.Name,
                dto.Category,
                dto.Price
            );

            var resultado = validator.Validate(comando);

            resultado.IsValid.Should().BeFalse();
            resultado.Errors.Should().NotBeEmpty();
            resultado.Errors.Should().HaveCount(1);
        }
         
        [Fact]
        public void IfCategoryEmpty_MustReturnError()
        {
            var id = Guid.NewGuid();
            var validator = new ValidatorBuilder()
                .WithProductPersisted(id)
                .Build();

            var dto = new ProductDtoBuilder()
                .WithCategory(string.Empty)
                .Build();

            var comando = new UpdateProductCommand(
                id,
                dto.ProductCode,
                dto.Name,
                dto.Category,
                dto.Price
            );

            var resultado = validator.Validate(comando);

            resultado.IsValid.Should().BeFalse();
            resultado.Errors.Should().NotBeEmpty();
            resultado.Errors.Should().HaveCount(1);
        }

        [Fact]
        public void IfProductCodeEmpty_MustReturnError()
        {
            var id = Guid.NewGuid();
            var validator = new ValidatorBuilder()
                .WithProductPersisted(id)
                .Build();

            var dto = new ProductDtoBuilder()
                .WithProductCode(0)
                .Build();

            var comando = new UpdateProductCommand(
                id,
                dto.ProductCode,
                dto.Name,
                dto.Category,
                dto.Price
            );

            var resultado = validator.Validate(comando);

            resultado.IsValid.Should().BeFalse();
            resultado.Errors.Should().NotBeEmpty();
            resultado.Errors.Should().HaveCount(1);
        }

        [Fact]
        public void IfPriceEmpty_MustReturnError()
        {
            var id = Guid.NewGuid();
            var validator = new ValidatorBuilder()
                .WithProductPersisted(id)
                .Build();

            var dto = new ProductDtoBuilder()
                .WithPrice(0)
                .Build();

            var comando = new UpdateProductCommand(
                id,
                dto.ProductCode,
                dto.Name,
                dto.Category,
                dto.Price
            );

            var resultado = validator.Validate(comando);

            resultado.IsValid.Should().BeFalse();
            resultado.Errors.Should().NotBeEmpty();
            resultado.Errors.Should().HaveCount(1);
        }

        [Fact]
        public void IfNewValuesGenerateAnDuplication_MustReturnError()
        {
            var id = Guid.NewGuid();
            var dto = new ProductDtoBuilder()
                .Build();
            var validator = new ValidatorBuilder()
                .WithDuplicateWithAnotherId(dto.Name, dto.ProductCode)
                .WithProductPersisted(id)
                .Build();

            var comando = new UpdateProductCommand(
                id,
                dto.ProductCode,
                dto.Name,
                dto.Category,
                dto.Price
            );

            var resultado = validator.Validate(comando);

            resultado.IsValid.Should().BeFalse();
            resultado.Errors.Should().NotBeEmpty();
            resultado.Errors.Should().HaveCount(1);
            resultado.Errors.First().ErrorMessage.Should().Be("The product must have unique name and code.");
        }

        [Fact]
        public void IfDoesntExists_MustReturnError()
        {
            var id = Guid.NewGuid();
            var validator = new ValidatorBuilder() 
                .Build();

            var dto = new ProductDtoBuilder() 
                .Build();

            var comando = new UpdateProductCommand(
                id,
                dto.ProductCode,
                dto.Name,
                dto.Category,
                dto.Price
            );

            var resultado = validator.Validate(comando);

            resultado.IsValid.Should().BeFalse();
            resultado.Errors.Should().NotBeEmpty();
            resultado.Errors.Should().HaveCount(1);
            resultado.Errors.First().ErrorMessage.Should().Be("The product must exists on database.");
        }

        [Fact]
        public void IfProductIsOk_MustReturnSuccess()
        {
            var id = Guid.NewGuid();
            var validator = new ValidatorBuilder()
                .WithProductPersisted(id)
                .Build();

            var dto = new ProductDtoBuilder()
                .Build();

            var comando = new UpdateProductCommand(
                id,
                dto.ProductCode,
                dto.Name,
                dto.Category,
                dto.Price
            );

            var resultado = validator.Validate(comando);

            resultado.IsValid.Should().BeTrue();
            resultado.Errors.Should().BeEmpty();
        }

        internal class ValidatorBuilder
        {
            private Mock<IProductRepository> _productRepositoryMock;

            public ValidatorBuilder()
            {
                _productRepositoryMock = new Mock<IProductRepository>();
            }

            public ValidatorBuilder WithDuplicate(string name, uint productCode, Guid id)
            {
                _productRepositoryMock.Setup(x => x.GetProductByNameAndCodeAsync(name, productCode))
                    .ReturnsAsync(new ProductBuilder()
                    .WithName(name)
                    .WithProductCode(productCode)
                    .WithId(id)
                    .Build());
                return this;
            }

            public ValidatorBuilder WithDuplicateWithAnotherId(string name, uint productCode)
            {
                _productRepositoryMock.Setup(x => x.GetProductByNameAndCodeAsync(name, productCode))
                    .ReturnsAsync(new ProductBuilder()
                    .WithName(name)
                    .WithProductCode(productCode) 
                    .Build());
                return this;
            }

            public ValidatorBuilder WithProductPersisted(Guid id)
            {
                _productRepositoryMock.Setup(x => x.GetByIdAsync(id))
                    .ReturnsAsync(new ProductBuilder()
                    .WithId(id) 
                    .Build());
                return this;
            }

            public UpdateProductCommandValidator Build()
                => new UpdateProductCommandValidator(_productRepositoryMock.Object);
        }
    }
}
