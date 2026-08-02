using AutoMapper;
using FluentAssertions;
using Moq;
using ProductSphere.Application.DTOs.ProductDtos;
using ProductSphere.Application.Interfaces.IRepositories;
using ProductSphere.Domain.Entities;
using ProductSphere.Domain.Exceptions;
using ProductSphere.Infrastructure.Services;

namespace ProductSphere.Application.Tests
{
    public class ProductServiceTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly ProductService _service;

        public ProductServiceTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _mapperMock = new Mock<IMapper>();

            _service = new ProductService(
                _unitOfWorkMock.Object,
                _mapperMock.Object);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnAllProducts()
        {
            var products = new List<Product>
            {
                new() { Id = 1, ProductName = "Laptop" },
                new() { Id = 2, ProductName = "Mouse" }
            };

            var dto = new List<ProductDto>
            {
                new() { Id = 1, ProductName = "Laptop" },
                new() { Id = 2, ProductName = "Mouse" }
            };

            _unitOfWorkMock.Setup(x => x.Products.GetAllAsync())
                           .ReturnsAsync(products);

            _mapperMock.Setup(x => x.Map<IEnumerable<ProductDto>>(products))
                       .Returns(dto);

            var result = await _service.GetAllAsync();

            result.Should().BeEquivalentTo(dto);

            _unitOfWorkMock.Verify(x => x.Products.GetAllAsync(), Times.Once);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnProduct_WhenProductExists()
        {
            var product = new Product
            {
                Id = 1,
                ProductName = "Laptop"
            };

            var dto = new ProductDto
            {
                Id = 1,
                ProductName = "Laptop"
            };

            _unitOfWorkMock.Setup(x => x.Products.GetByIdAsync(1))
                           .ReturnsAsync(product);

            _mapperMock.Setup(x => x.Map<ProductDto>(product))
                       .Returns(dto);

            var result = await _service.GetByIdAsync(1);

            result.Should().BeEquivalentTo(dto);

            _unitOfWorkMock.Verify(x => x.Products.GetByIdAsync(1), Times.Once);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldThrowNotFoundException_WhenProductDoesNotExist()
        {
            _unitOfWorkMock.Setup(x => x.Products.GetByIdAsync(1))
                           .ReturnsAsync((Product?)null);

            Func<Task> act = async () => await _service.GetByIdAsync(1);

            await act.Should().ThrowAsync<NotFoundException>();
        }

        [Fact]
        public async Task CreateAsync_ShouldCreateProduct()
        {
            var createDto = new CreateProductDto
            {
                ProductName = "Laptop"
            };

            var product = new Product
            {
                ProductName = "Laptop"
            };

            _unitOfWorkMock.Setup(x => x.Products.ExistsByNameAsync(createDto.ProductName))
                           .ReturnsAsync(false);

            _mapperMock.Setup(x => x.Map<Product>(createDto))
                       .Returns(product);

            await _service.CreateAsync(createDto);

            _unitOfWorkMock.Verify(x => x.Products.AddAsync(It.IsAny<Product>()), Times.Once);

            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(), Times.Once);
        }


        [Fact]
        public async Task CreateAsync_ShouldThrowBadRequestException_WhenProductAlreadyExists()
        {
            var createDto = new CreateProductDto
            {
                ProductName = "Laptop"
            };

            _unitOfWorkMock.Setup(x => x.Products.ExistsByNameAsync(createDto.ProductName))
                           .ReturnsAsync(true);

            Func<Task> act = async () => await _service.CreateAsync(createDto);

            await act.Should().ThrowAsync<BadRequestException>();

            _unitOfWorkMock.Verify(x => x.Products.AddAsync(It.IsAny<Product>()), Times.Never);
        }

        [Fact]
        public async Task UpdateAsync_ShouldUpdateProduct()
        {
            var dto = new UpdateProductDto
            {
                Id = 1,
                ProductName = "Updated Laptop"
            };

            var product = new Product
            {
                Id = 1,
                ProductName = "Laptop"
            };

            _unitOfWorkMock.Setup(x => x.Products.GetByIdAsync(dto.Id))
                           .ReturnsAsync(product);

            await _service.UpdateAsync(dto);

            _unitOfWorkMock.Verify(x => x.Products.Update(product), Times.Once);
            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_ShouldThrowNotFoundException_WhenProductDoesNotExist()
        {
            var dto = new UpdateProductDto
            {
                Id = 1,
                ProductName = "Laptop"
            };

            _unitOfWorkMock.Setup(x => x.Products.GetByIdAsync(dto.Id))
                           .ReturnsAsync((Product?)null);

            Func<Task> act = async () => await _service.UpdateAsync(dto);

            await act.Should().ThrowAsync<NotFoundException>();
        }

        [Fact]
        public async Task DeleteAsync_ShouldDeleteProduct()
        {
            var product = new Product
            {
                Id = 1,
                ProductName = "Laptop"
            };

            _unitOfWorkMock.Setup(x => x.Products.GetByIdAsync(1))
                           .ReturnsAsync(product);

            await _service.DeleteAsync(1);

            _unitOfWorkMock.Verify(x => x.Products.Delete(product), Times.Once);
            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_ShouldThrowNotFoundException_WhenProductDoesNotExist()
        {
            _unitOfWorkMock.Setup(x => x.Products.GetByIdAsync(1))
                           .ReturnsAsync((Product?)null);

            Func<Task> act = async () => await _service.DeleteAsync(1);

            await act.Should().ThrowAsync<NotFoundException>();
        }
    }
}

 