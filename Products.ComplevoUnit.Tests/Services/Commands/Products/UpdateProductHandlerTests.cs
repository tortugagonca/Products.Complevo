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
    [Trait("Unity", nameof(UpdateProductHandler))]
    public class UpdateProductHandlerTests
    {
        private readonly Mock<IValidator<UpdateProductCommand>> _validator;
        private readonly Mock<IProductRepository> _repository;
        public UpdateProductHandlerTests()
        {  
            _validator = new Mock<IValidator<UpdateProductCommand>>();
            _repository = new Mock<IProductRepository>();
            _validator.Setup(x => x.ValidateAsync(It.IsAny<UpdateProductCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult());
        }
        private UpdateProductHandler GenerateHandler()
            => new UpdateProductHandler( 
                _validator.Object,
                _repository.Object);


        [Fact]
        public async Task IfErrorOnValidation_ShouldThrowException()
        {
            var errorMessage = "error on property";
            var id = Guid.NewGuid();
            var dto = new ProductDtoBuilder().Build();
            var command = new UpdateProductCommand(id, dto.ProductCode, dto.Name, dto.Category, dto.Price);

            _validator.Setup(x => x.ValidateAsync(command, It.IsAny<CancellationToken>()))
               .ReturnsAsync(new ValidationResult(new ValidationFailure[1] { new ValidationFailure("id", errorMessage) }));

            var handler = GenerateHandler();
            var exception = await Assert.ThrowsAsync<ProductsComplevoValidationException>(async () =>
            {
                await handler.Handle(command, It.IsAny<CancellationToken>());
            });

            exception.Errors.First().ErrorMessage.Should().Be(errorMessage);
        }

        [Fact]
        public async Task IfErrorOnPersist_ShouldThrowException()
        {
            var id = Guid.NewGuid();
            var dto = new ProductDtoBuilder().Build();
            var command = new UpdateProductCommand(id, dto.ProductCode, dto.Name, dto.Category, dto.Price);
            var product = new ProductBuilder()
                .WithCategory(dto.Category)
                .WithName(dto.Name)
                .WithPrice(dto.Price)
                .WithProductCode(dto.ProductCode)
                .WithId(id)
                .Build();
             
            _repository.Setup(x => x.GetByIdWithTrackingAsync(id))
                .ReturnsAsync(product);
            _repository.Setup(x => x.SaveChangesAsync())
                .ReturnsAsync(0);

            var handler = GenerateHandler();
            var exception = await Assert.ThrowsAsync<ProductsComplevoValidationException>(async () =>
            {
                await handler.Handle(command, It.IsAny<CancellationToken>());
            });

            exception.Message.Should().Be(ProductsComplevoResource.NoRecordsPersisted);
        }

        [Fact]
        public async Task IfSuccesOnSave_ShouldReturnNewId()
        {
            var id = Guid.NewGuid();
            var dto = new ProductDtoBuilder().Build();
            var command = new UpdateProductCommand(id, dto.ProductCode, dto.Name, dto.Category, dto.Price);
            var product = new ProductBuilder() 
                .WithId(id)
                .Build();
             
            _repository.Setup(x => x.GetByIdWithTrackingAsync(id))
                .ReturnsAsync(product);
            _repository.Setup(x => x.SaveChangesAsync())
                .ReturnsAsync(1);

            var handler = GenerateHandler();
            await handler.Handle(command, It.IsAny<CancellationToken>()); 
        }
    }
}
