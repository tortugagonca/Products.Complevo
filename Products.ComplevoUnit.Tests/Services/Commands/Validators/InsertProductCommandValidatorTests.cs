﻿using FluentAssertions;
using Moq;
using Products.Complevo.Application.Core.Interfaces.Repository;
using Products.Complevo.Service.Commands.Products;
using Products.Complevo.Service.Commands.Validator;
using Products.Complevo.Unit.Tests.Builder;
using Xunit;

namespace Products.Complevo.Unit.Tests.Services.Commands.Validators
{
    [Trait("Unity", nameof(InsertProductCommandValidator))]
    public class InsertProductCommandValidatorTests
    { 
        [Fact]
        public void IfNameEmpty_MustReturnError()
        {
            var validator = new ValidatorBuilder()
                .Build();

            var dto = new ProductDtoBuilder() 
                .WithName(string.Empty)
                .Build();

            var comando = new InsertProductCommand(
                dto.ProductCode,
                dto.Name,
                dto.Category,
                dto.Price
            );

            var resultado = validator.Validate(comando);

            resultado.IsValid.Should().BeFalse();
            resultado.Errors.Should().NotBeEmpty();
        }
         
        [Fact]
        public void IfCategoryEmpty_MustReturnError()
        {
            var validator = new ValidatorBuilder()
                .Build();

            var dto = new ProductDtoBuilder()
                .WithCategory(string.Empty)
                .Build();

            var comando = new InsertProductCommand(
                dto.ProductCode,
                dto.Name,
                dto.Category,
                dto.Price
            );

            var resultado = validator.Validate(comando);

            resultado.IsValid.Should().BeFalse();
            resultado.Errors.Should().NotBeEmpty();
        }

        [Fact]
        public void IfProductCodeEmpty_MustReturnError()
        {
            var validator = new ValidatorBuilder()
                .Build();

            var dto = new ProductDtoBuilder()
                .WithProductCode(0)
                .Build();

            var comando = new InsertProductCommand(
                dto.ProductCode,
                dto.Name,
                dto.Category,
                dto.Price
            );

            var resultado = validator.Validate(comando);

            resultado.IsValid.Should().BeFalse();
            resultado.Errors.Should().NotBeEmpty();
        }

        [Fact]
        public void IfPriceEmpty_MustReturnError()
        {
            var validator = new ValidatorBuilder()
                .Build();

            var dto = new ProductDtoBuilder()
                .WithPrice(0)
                .Build();

            var comando = new InsertProductCommand(
                dto.ProductCode,
                dto.Name,
                dto.Category,
                dto.Price
            );

            var resultado = validator.Validate(comando);

            resultado.IsValid.Should().BeFalse();
            resultado.Errors.Should().NotBeEmpty();
        }

        [Fact]
        public void IfDuplicated_MustReturnError()
        {
            var dto = new ProductDtoBuilder()
                .Build();

            var validator = new ValidatorBuilder()
                .WithDuplicate(dto.Name, dto.ProductCode)
                .Build();

            var comando = new InsertProductCommand(
                dto.ProductCode,
                dto.Name,
                dto.Category,
                dto.Price
            );

            var resultado = validator.Validate(comando);

            resultado.IsValid.Should().BeFalse();
            resultado.Errors.Should().NotBeEmpty();
        }

        [Fact]
        public void IfProductIsOk_MustReturnSuccess()
        { 
            var validator = new ValidatorBuilder() 
                .Build();

            var dto = new ProductDtoBuilder() 
                .Build();

            var comando = new InsertProductCommand(
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

            public ValidatorBuilder WithDuplicate(string name, uint productCode)
            {
                _productRepositoryMock.Setup(x => x.GetProductByNameAndCodeAsync(name, productCode))
                    .ReturnsAsync(new ProductBuilder()
                    .WithName(name)
                    .WithProductCode(productCode)
                    .Build());
                return this;
            }

            public InsertProductCommandValidator Build()
                => new InsertProductCommandValidator(_productRepositoryMock.Object);
        }
    }
}
