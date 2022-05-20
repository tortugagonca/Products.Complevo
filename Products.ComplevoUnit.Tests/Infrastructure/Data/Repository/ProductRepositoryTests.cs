using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using Products.Complevo.Application.Core.Dto;
using Products.Complevo.Application.Core.Services.Decorators;
using Products.Complevo.Domain.Models;
using Products.Complevo.Infrastructure.Data;
using Products.Complevo.Infrastructure.Data.Repository;
using Products.Complevo.Unit.Tests.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Products.Complevo.Unit.Tests.Infrastructure.Data.Repository
{
    public class ProductRepositoryTests
    {
        private readonly DbContextOptions<ProductsComplevoContext> _contextOptions = new DbContextOptionsBuilder<ProductsComplevoContext>()
            .UseInMemoryDatabase(databaseName: "ProductsComplevoContext")
            .Options;
        private readonly Mock<IFilterProductsFactory> _filterProductsFactory;
        public ProductRepositoryTests()
        {
            _filterProductsFactory = new Mock<IFilterProductsFactory>();
        }
        [Fact]
        public void Must_Insert_New_Product()
        {
            using (var context = new ProductsComplevoContext(_contextOptions))
            {
                CreateNewProduct(context);

                Assert.True(context.Products.AnyAsync().GetAwaiter().GetResult());
            }
        }

        [Fact]
        public void Must_Update_Product_Persisted()
        {
            using (var context = new ProductsComplevoContext(_contextOptions))
            {
                CreateNewProduct(context);

                var productPersisted = context.Products.FirstAsync().GetAwaiter().GetResult();

                productPersisted.Name = "new name of product";
                context.SaveChanges();

                var productUpdated = context.Products.FirstAsync().GetAwaiter().GetResult();

                Assert.Equal("new name of product", productUpdated.Name);
            }

        }

        [Fact]
        public async Task If_Exists_Must_Find()
        {
            using (var context = new ProductsComplevoContext(_contextOptions))
            {
                CreateNewProduct(context);

                var productPersisted = context.Products.FirstAsync().GetAwaiter().GetResult();

                var repository = new ProductRepository(context, _filterProductsFactory.Object);

                var productFound = await repository.GetByIdAsync(productPersisted.Id);

                productFound.Should().NotBeNull();
                productFound.Id.Should().Equals(productPersisted.Id);

                productFound = await repository.GetByIdWithTrackingAsync(productPersisted.Id);

                productFound.Should().NotBeNull();
                productFound.Id.Should().Equals(productPersisted.Id);
            }
        }

        [Fact]
        public async Task GetProductByNameAndCodeAsync_IfExists_Must_Return()
        {
            using (var context = new ProductsComplevoContext(_contextOptions))
            {
                CreateNewProduct(context);

                var productPersisted = context.Products.FirstAsync().GetAwaiter().GetResult();
                _filterProductsFactory.Setup(x => x.GetProductsAsync(It.IsAny<ProductDto>(), It.IsAny<IQueryable<Product>>()))
                    .ReturnsAsync(context.Products.AsEnumerable());
                var repository = new ProductRepository(context, _filterProductsFactory.Object);

                var productFound = await repository.GetProductByNameAndCodeAsync(productPersisted.Name, productPersisted.ProductCode);

                productFound.Should().NotBeNull();
                productFound.Id.Should().Equals(productPersisted.Id);
                productFound.Name.Should().Equals(productPersisted.Name);
                productFound.ProductCode.Should().Equals(productPersisted.ProductCode);
            }
        }


        [Fact]
        public async Task DeleteAsync_IfExists_Must_Delete()
        {
            using (var context = new ProductsComplevoContext(_contextOptions))
            {
                CreateNewProduct(context);

                var productPersisted = context.Products.FirstAsync().GetAwaiter().GetResult();

                var repository = new ProductRepository(context, _filterProductsFactory.Object);

                var productFound = await repository.GetByIdWithTrackingAsync(productPersisted.Id);

                productFound.Should().NotBeNull();
                productFound.Id.Should().Equals(productPersisted.Id);

                var countOfRegistersDeleted = await repository.DeleteAsync(productFound);
                countOfRegistersDeleted.Should().Equals(1);

                productFound = await repository.GetByIdAsync(productPersisted.Id);
                productFound.Should().BeNull(); 
            }
        }

        private void CreateNewProduct(ProductsComplevoContext context)
        {
            var product = new ProductBuilder()
                .Build();
            context.Add(product);
            context.SaveChanges();
        }
    }
}
