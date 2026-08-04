using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using ProductSphere.Application.Interfaces.IRepositories;
using ProductSphere.Infrastructure.Data;

namespace ProductSphere.Infrastructure.Tests.Data
{
    public class UnitOfWorkTests
    {
        private static ApplicationDbContext CreateContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            return new ApplicationDbContext(options);
        }

        [Fact]
        public void Constructor_Should_Initialize_Repositories()
        {
            // Arrange
            var context = CreateContext();

            var productRepository = new Mock<IProductRepository>();
            var itemRepository = new Mock<IItemRepository>();

            // Act
            var unitOfWork = new UnitOfWork(
                context,
                productRepository.Object,
                itemRepository.Object);

            // Assert
            unitOfWork.Products.Should().Be(productRepository.Object);
            unitOfWork.Items.Should().Be(itemRepository.Object);
        }

        [Fact]
        public async Task SaveChangesAsync_Should_Return_Zero_When_No_Changes()
        {
            // Arrange
            var context = CreateContext();

            var productRepository = new Mock<IProductRepository>();
            var itemRepository = new Mock<IItemRepository>();

            var unitOfWork = new UnitOfWork(
                context,
                productRepository.Object,
                itemRepository.Object);

            // Act
            var result = await unitOfWork.SaveChangesAsync();

            // Assert
            result.Should().Be(0);
        }

        [Fact]
        public void Dispose_Should_Not_Throw_Exception()
        {
            // Arrange
            var context = CreateContext();

            var productRepository = new Mock<IProductRepository>();
            var itemRepository = new Mock<IItemRepository>();

            var unitOfWork = new UnitOfWork(
                context,
                productRepository.Object,
                itemRepository.Object);

            // Act
            var action = () => unitOfWork.Dispose();

            // Assert
            action.Should().NotThrow();
        }
    }
}