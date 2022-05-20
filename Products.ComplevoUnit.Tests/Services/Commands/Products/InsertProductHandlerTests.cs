using AutoMapper;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Moq;
using Products.Complevo.Application.Core.Exceptions;
using Products.Complevo.Application.Core.Interfaces.Repository;
using Products.Complevo.Application.Core.Resources;
using Products.Complevo.Domain.Models;
using Products.Complevo.Service.Commands.Products;
using Products.Complevo.Unit.Tests.Builder;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Products.Complevo.Unit.Tests.Services.Commands.Products
{
    [Trait("Unity", nameof(InsertProductHandler))]
    public class InsertProductHandlerTests
    {
        private readonly Mock<IValidator<InsertProductCommand>> _validator;
        private readonly Mock<IProductRepository> _repository;
        private readonly Mock<IMapper> _mapper;

        public InsertProductHandlerTests()
        {
            _mapper = new Mock<IMapper>();
            _validator = new Mock<IValidator<InsertProductCommand>>();
            _repository = new Mock<IProductRepository>();
            _validator.Setup(x => x.ValidateAsync(It.IsAny<InsertProductCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult());
        }

        private InsertProductHandler GenerateHandler()
            => new InsertProductHandler(
                _mapper.Object,
                _validator.Object,
                _repository.Object);

        [Fact]
        public async Task IfErrorOnValidation_ShouldThrowException()
        {
            var errorMessage = "error on property";
            var id = Guid.NewGuid();
            var dto = new ProductDtoBuilder().Build();
            var command = new InsertProductCommand(dto.ProductCode, dto.Name, dto.Category, dto.Price);

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
        public async Task IfErrorOnSave_ShouldThrowException()
        {  
            var dto = new ProductDtoBuilder().Build();
            var command = new InsertProductCommand(dto.ProductCode, dto.Name, dto.Category, dto.Price);
            var product = new ProductBuilder()
                .WithCategory(dto.Category).WithName(dto.Name).WithPrice(dto.Price).WithProductCode(dto.ProductCode)
                .Build();

            _mapper.Setup(x => x.Map<Product>(command))
                .Returns(product);
            _repository.Setup(x => x.InsertAsync(product))
                .ReturnsAsync((Product)default);

            var handler = GenerateHandler();
            var exception = await Assert.ThrowsAsync<ProductsComplevoValidationException>(async () =>
            {
                await handler.Handle(command, It.IsAny<CancellationToken>());
            });

            exception.Message.Should().Be(ProductsComplevoResource.NoProductGenerated);
        }

        [Fact]
        public async Task IfErrorOnPersist_ShouldThrowException()
        {
            var id = Guid.NewGuid();
            var dto = new ProductDtoBuilder().Build();
            var command = new InsertProductCommand(dto.ProductCode, dto.Name, dto.Category, dto.Price);
            var product = new ProductBuilder()
                .WithCategory(dto.Category)
                .WithName(dto.Name)
                .WithPrice(dto.Price)
                .WithProductCode(dto.ProductCode)
                .WithId(id)
                .Build();

            _mapper.Setup(x => x.Map<Product>(command))
                .Returns(product);
            _repository.Setup(x => x.InsertAsync(product))
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
            var command = new InsertProductCommand(dto.ProductCode, dto.Name, dto.Category, dto.Price);
            var product = new ProductBuilder()
                .WithCategory(dto.Category)
                .WithName(dto.Name)
                .WithPrice(dto.Price)
                .WithProductCode(dto.ProductCode)
                .WithId(id)
                .Build();

            _mapper.Setup(x => x.Map<Product>(command))
                .Returns(product);
            _repository.Setup(x => x.InsertAsync(product))
                .ReturnsAsync(product);
            _repository.Setup(x => x.SaveChangesAsync())
                .ReturnsAsync(1);

            var handler = GenerateHandler();
            var newId = await handler.Handle(command, It.IsAny<CancellationToken>());
            newId.Should().Equals(id);
        }
    }
}
