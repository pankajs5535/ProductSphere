using FluentAssertions;
using ProductSphere.Domain.Entities;
using ProductSphere.Infrastructure.Tests.Helpers;

namespace ProductSphere.Infrastructure.Tests.Data
{
    public class ApplicationDbContextTests
    {
        [Fact]
        public async Task SaveChangesAsync_Should_Save_Product()
        {
            // Arrange
            using var context = DbContextFactory.Create();

            var product = new Product
            {
                ProductName = "Laptop",
                CreatedBy = "Admin",
                CreatedOn = DateTime.UtcNow
            };

            // Act
            await context.Products.AddAsync(product);
            await context.SaveChangesAsync();

            var result = await context.Products.FindAsync(product.Id);

            // Assert
            result.Should().NotBeNull();
            result!.ProductName.Should().Be("Laptop");
            result.CreatedBy.Should().Be("Admin");
        }
    }
}