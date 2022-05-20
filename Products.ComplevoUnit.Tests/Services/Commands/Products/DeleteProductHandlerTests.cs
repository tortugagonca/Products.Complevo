using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Moq;
using Products.Complevo.Application.Core.Exceptions;
using Products.Complevo.Application.Core.Interfaces.Repository;
using Products.Complevo.Application.Core.Resources;
using Products.Complevo.Service.Commands.Products;
using Products.Complevo.Unit.Tests.Builder;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Products.Complevo.Unit.Tests.Services.Commands.Products
{
    [Trait("Unity", nameof(DeleteProductHandler))]
    public class DeleteProductHandlerTests
    {

        private readonly Mock<IValidator<DeleteProductCommand>> _validator;
        private readonly Mock<IProductRepository> _repository;

        public DeleteProductHandlerTests()
        {
            _validator = new Mock<IValidator<DeleteProductCommand>>();
            _repository = new Mock<IProductRepository>();
            _validator.Setup(x => x.ValidateAsync(It.IsAny<DeleteProductCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult());
        }

        private DeleteProductHandler GenerateHandler()
            => new DeleteProductHandler(_validator.Object,
                _repository.Object);

        [Fact]
        public async Task IfErrorOnValidation_ShouldThrowException()
        {
            var errorMessage = "error on property";
            var id = Guid.NewGuid();
            var command = new DeleteProductCommand(id);

            _validator.Setup(x => x.ValidateAsync(command, It.IsAny<CancellationToken>()))
               .ReturnsAsync(new ValidationResult(new ValidationFailure[1] { new ValidationFailure("id",errorMessage ) }));

            var handler = GenerateHandler();
            var exception = await Assert.ThrowsAsync<ProductsComplevoValidationException>(async () =>
            {
                await handler.Handle(command, It.IsAny<CancellationToken>());
            }); 

            exception.Errors.First().ErrorMessage.Should().Be(errorMessage); 
        }

        [Fact]
        public async Task IfProductExists_ShouldDeleteId()
        {
            var id = Guid.NewGuid();
            var command = new DeleteProductCommand(id);
            var product = new ProductBuilder()
                .WithId(id)
                .Build();
            _repository.Setup(x => x.GetByIdAsync(id))
                .ReturnsAsync(product);
            _repository.Setup(x => x.DeleteAsync(product))
                .ReturnsAsync(1);
            var handler = GenerateHandler();
            await handler.Handle(command, It.IsAny<CancellationToken>());
        }

        [Fact]
        public async Task IfRepositoryDoesntDeleteARecord_ShouldThrows()
        {
            var id = Guid.NewGuid();
            var command = new DeleteProductCommand(id);
            var product = new ProductBuilder()
                .WithId(id)
                .Build();
            _repository.Setup(x => x.GetByIdAsync(id))
                .ReturnsAsync(product);
            _repository.Setup(x => x.DeleteAsync(product))
                .ReturnsAsync(0);
            var handler = GenerateHandler();
            var exception = await Assert.ThrowsAsync<ProductsComplevoValidationException>(async () =>
            {
                await handler.Handle(command, It.IsAny<CancellationToken>());
            });
            exception.Message.Should().Be(ProductsComplevoResource.NoRecordsDeleted);
        }
    }
}
