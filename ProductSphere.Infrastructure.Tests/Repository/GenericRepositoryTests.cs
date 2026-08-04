using FluentAssertions;
using ProductSphere.Domain.Entities;
using ProductSphere.Infrastructure.Data.Repositories;
using ProductSphere.Infrastructure.Tests.Helpers;

namespace ProductSphere.Infrastructure.Tests.Repositories
{
    public class GenericRepositoryTests
    {
        [Fact]
        public async Task AddAsync_Should_Add_Product()
        {
            // Arrange
            using var context = DbContextFactory.Create();

            var repository = new GenericRepository<Product>(context);

            var product = new Product
            {
                ProductName = "Laptop",
                CreatedBy = "Admin",
                CreatedOn = DateTime.UtcNow
            };

            // Act
            await repository.AddAsync(product);
            await context.SaveChangesAsync();

            // Assert
            var result = await context.Products.FindAsync(product.Id);

            result.Should().NotBeNull();
            result!.ProductName.Should().Be("Laptop");
        }

        [Fact]
        public async Task GetAllAsync_Should_Return_All_Products()
        {
            // Arrange
            using var context = DbContextFactory.Create();

            context.Products.AddRange(
                new Product
                {
                    ProductName = "Laptop",
                    CreatedBy = "Admin",
                    CreatedOn = DateTime.UtcNow
                },
                new Product
                {
                    ProductName = "Mouse",
                    CreatedBy = "Admin",
                    CreatedOn = DateTime.UtcNow
                });

            await context.SaveChangesAsync();

            var repository = new GenericRepository<Product>(context);

            // Act
            var result = await repository.GetAllAsync();

            // Assert
            result.Should().HaveCount(2);
        }

        [Fact]
        public async Task GetByIdAsync_Should_Return_Product()
        {
            using var context = DbContextFactory.Create();

            var product = new Product
            {
                ProductName = "Laptop",
                CreatedBy = "Admin",
                CreatedOn = DateTime.UtcNow
            };

            context.Products.Add(product);
            await context.SaveChangesAsync();

            var repository = new GenericRepository<Product>(context);

            var result = await repository.GetByIdAsync(product.Id);

            result.Should().NotBeNull();
            result!.ProductName.Should().Be("Laptop");
        }

        [Fact]
        public async Task FindAsync_Should_Return_Matching_Product()
        {
            using var context = DbContextFactory.Create();

            context.Products.AddRange(
                new Product
                {
                    ProductName = "Laptop",
                    CreatedBy = "Admin",
                    CreatedOn = DateTime.UtcNow
                },
                new Product
                {
                    ProductName = "Mouse",
                    CreatedBy = "Admin",
                    CreatedOn = DateTime.UtcNow
                });

            await context.SaveChangesAsync();

            var repository = new GenericRepository<Product>(context);

            var result = await repository.FindAsync(x => x.ProductName == "Laptop");

            result.Should().ContainSingle();
        }

        [Fact]
        public async Task Update_Should_Modify_Product()
        {
            using var context = DbContextFactory.Create();

            var product = new Product
            {
                ProductName = "Laptop",
                CreatedBy = "Admin",
                CreatedOn = DateTime.UtcNow
            };

            context.Products.Add(product);
            await context.SaveChangesAsync();

            var repository = new GenericRepository<Product>(context);

            product.ProductName = "Gaming Laptop";

            repository.Update(product);
            await context.SaveChangesAsync();

            var result = await context.Products.FindAsync(product.Id);

            result!.ProductName.Should().Be("Gaming Laptop");
        }

        [Fact]
        public async Task Delete_Should_Remove_Product()
        {
            using var context = DbContextFactory.Create();

            var product = new Product
            {
                ProductName = "Laptop",
                CreatedBy = "Admin",
                CreatedOn = DateTime.UtcNow
            };

            context.Products.Add(product);
            await context.SaveChangesAsync();

            var repository = new GenericRepository<Product>(context);

            repository.Delete(product);
            await context.SaveChangesAsync();

            var result = await context.Products.FindAsync(product.Id);

            result.Should().BeNull();
        }
    }
}