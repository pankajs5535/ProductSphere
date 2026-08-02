using AutoMapper;
using FluentAssertions;
using Moq;
using ProductSphere.Application.DTOs.ItemDtos;
using ProductSphere.Application.Interfaces.IRepositories;
using ProductSphere.Domain.Entities;
using ProductSphere.Domain.Exceptions;
using ProductSphere.Infrastructure.Services;

namespace ProductSphere.Application.Tests
{
    public class ItemServiceTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IItemRepository> _itemRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly ItemService _service;

        public ItemServiceTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _itemRepositoryMock = new Mock<IItemRepository>();
            _mapperMock = new Mock<IMapper>();

            _unitOfWorkMock
                .Setup(x => x.Items)
                .Returns(_itemRepositoryMock.Object);

            _service = new ItemService(
                _unitOfWorkMock.Object,
                _mapperMock.Object);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnAllItems()
        {
            var items = new List<Item>
            {
                new Item { Id = 1, ProductId = 1, Quantity = 10 },
                new Item { Id = 2, ProductId = 1, Quantity = 20 }
            };

            var itemDtos = new List<ItemDto>
            {
                new ItemDto { Id = 1, ProductId = 1, Quantity = 10 },
                new ItemDto { Id = 2, ProductId = 1, Quantity = 20 }
            };

            _itemRepositoryMock
                .Setup(x => x.GetAllAsync())
                .ReturnsAsync(items);

            _mapperMock
                .Setup(x => x.Map<IEnumerable<ItemDto>>(items))
                .Returns(itemDtos);

            var result = await _service.GetAllAsync();

            result.Should().BeEquivalentTo(itemDtos);

            _itemRepositoryMock.Verify(x => x.GetAllAsync(), Times.Once);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnItem_WhenItemExists()
        {
            var item = new Item
            {
                Id = 1,
                ProductId = 1,
                Quantity = 10
            };

            var dto = new ItemDto
            {
                Id = 1,
                ProductId = 1,
                Quantity = 10
            };

            _itemRepositoryMock
                .Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync(item);

            _mapperMock
                .Setup(x => x.Map<ItemDto>(item))
                .Returns(dto);

            var result = await _service.GetByIdAsync(1);

            result.Should().BeEquivalentTo(dto);

            _itemRepositoryMock.Verify(x => x.GetByIdAsync(1), Times.Once);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldThrowNotFoundException_WhenItemDoesNotExist()
        {
            _itemRepositoryMock
                .Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync((Item?)null);

            Func<Task> act = async () => await _service.GetByIdAsync(1);

            await act.Should().ThrowAsync<NotFoundException>();
        }

        [Fact]
        public async Task CreateAsync_ShouldCreateItem()
        {
            var dto = new CreateItemDto
            {
                ProductId = 1,
                Quantity = 10
            };

            var item = new Item
            {
                ProductId = 1,
                Quantity = 10
            };

            _mapperMock
                .Setup(x => x.Map<Item>(dto))
                .Returns(item);

            await _service.CreateAsync(dto);

            _itemRepositoryMock.Verify(x => x.AddAsync(It.IsAny<Item>()), Times.Once);

            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_ShouldUpdateItem()
        {
            var dto = new UpdateItemDto
            {
                Id = 1,
                ProductId = 1,
                Quantity = 25
            };

            var item = new Item
            {
                Id = 1,
                ProductId = 1,
                Quantity = 10
            };

            _itemRepositoryMock
                .Setup(x => x.GetByIdAsync(dto.Id))
                .ReturnsAsync(item);

            await _service.UpdateAsync(dto);

            _mapperMock.Verify(x => x.Map(dto, item), Times.Once);

            _itemRepositoryMock.Verify(x => x.Update(item), Times.Once);

            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_ShouldThrowNotFoundException_WhenItemDoesNotExist()
        {
            var dto = new UpdateItemDto
            {
                Id = 1,
                ProductId = 1,
                Quantity = 25
            };

            _itemRepositoryMock
                .Setup(x => x.GetByIdAsync(dto.Id))
                .ReturnsAsync((Item?)null);

            Func<Task> act = async () => await _service.UpdateAsync(dto);

            await act.Should().ThrowAsync<NotFoundException>();

            _itemRepositoryMock.Verify(x => x.Update(It.IsAny<Item>()), Times.Never);
        }

        [Fact]
        public async Task DeleteAsync_ShouldDeleteItem()
        {
            var item = new Item
            {
                Id = 1,
                ProductId = 1,
                Quantity = 10
            };

            _itemRepositoryMock
                .Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync(item);

            await _service.DeleteAsync(1);

            _itemRepositoryMock.Verify(x => x.Delete(item), Times.Once);

            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_ShouldThrowNotFoundException_WhenItemDoesNotExist()
        {
            _itemRepositoryMock
                .Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync((Item?)null);

            Func<Task> act = async () => await _service.DeleteAsync(1);

            await act.Should().ThrowAsync<NotFoundException>();

            _itemRepositoryMock.Verify(x => x.Delete(It.IsAny<Item>()), Times.Never);
        }
    }
}