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
    [Trait("Unity", nameof(DeleteProductCommandValidator))]
    public class DeleteProductCommandValidatorTests
    {
        [Fact]
        public void IfDoesntExists_MustReturnError()
        {
            var id = Guid.NewGuid();
            var validator = new ValidatorBuilder()
                .Build();

            var comando = new DeleteProductCommand(
                id
            );

            var resultado = validator.Validate(comando);

            resultado.IsValid.Should().BeFalse();
            resultado.Errors.Should().NotBeEmpty();
            resultado.Errors.Should().HaveCount(1);
            resultado.Errors.First().ErrorMessage.Should().Be("The product must exists on database.");
        }

        [Fact]
        public void IfExists_MustReturnSuccess()
        {
            var id = Guid.NewGuid();
            var validator = new ValidatorBuilder()
                .WithProductPersisted(id)
                .Build();

            var comando = new DeleteProductCommand(
                id
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

            public ValidatorBuilder WithProductPersisted(Guid id)
            {
                _productRepositoryMock.Setup(x => x.GetByIdAsync(id))
                    .ReturnsAsync(new ProductBuilder()
                    .WithId(id)
                    .Build());
                return this;
            }

            public DeleteProductCommandValidator Build()
                => new DeleteProductCommandValidator(_productRepositoryMock.Object);
        }
    }
}
