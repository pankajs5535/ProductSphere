using FluentAssertions;
using ProductSphere.Application.DTOs.AuthDtos;
using ProductSphere.Application.DTOs.ItemDtos;
using ProductSphere.Application.DTOs.ProductDtos;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace ProductSphere.API.Tests
{
    public class ItemControllerTests
        : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _client;

        public ItemControllerTests(CustomWebApplicationFactory factory)
        {
            _client = factory.CreateClient();
        }

        private async Task AuthenticateAsync()
        {
            var register = new RegisterRequestDto
            {
                UserName = "admin",
                Email = $"admin{Guid.NewGuid()}@test.com",
                Password = "Password@123",
                ConfirmPassword = "Password@123",
                //RoleId = Guid.NewGuid()
                RoleId = Guid.Parse("11111111-1111-1111-1111-111111111111")
            };

            await _client.PostAsJsonAsync("/api/Auth/register", register);

            var login = new LoginRequestDto
            {
                Email = register.Email,
                Password = register.Password
            };

            var loginResponse = await _client.PostAsJsonAsync(
                "/api/Auth/login",
                login);

            loginResponse.EnsureSuccessStatusCode();

            var auth = await loginResponse.Content
                .ReadFromJsonAsync<AuthResponseDto>();

            _client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue(
                    "Bearer",
                    auth!.AccessToken);
        }

        private async Task<int> CreateProductAsync()
        {
            var dto = new CreateProductDto
            {
                ProductName = "Laptop",
                CreatedBy = "IntegrationTest"
            };

            await _client.PostAsJsonAsync("/api/Product", dto);

            var products = await _client.GetFromJsonAsync<List<ProductDto>>(
                "/api/Product");

            return products!.Last().Id;
        }

        [Fact]
        public async Task GetAll_ShouldReturnOk()
        {
            await AuthenticateAsync();

            var response = await _client.GetAsync("/api/Item");

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task GetById_ShouldReturnNotFound_WhenItemDoesNotExist()
        {
            await AuthenticateAsync();

            var response = await _client.GetAsync("/api/Item/99999");

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Create_ShouldReturnOk()
        {
            await AuthenticateAsync();

            var productId = await CreateProductAsync();

            var dto = new CreateItemDto
            {
                ProductId = productId,
                Quantity = 10
            };

            var response = await _client.PostAsJsonAsync(
                "/api/Item",
                dto);

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task Create_ShouldReturnBadRequest_WhenModelIsInvalid()
        {
            await AuthenticateAsync();

            var dto = new CreateItemDto();

            var response = await _client.PostAsJsonAsync(
                "/api/Item",
                dto);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Update_ShouldReturnOk()
        {
            await AuthenticateAsync();

            var productId = await CreateProductAsync();

            var createDto = new CreateItemDto
            {
                ProductId = productId,
                Quantity = 10
            };

            await _client.PostAsJsonAsync(
                "/api/Item",
                createDto);

            var items = await _client.GetFromJsonAsync<List<ItemDto>>(
                "/api/Item");

            var item = items!.Last();

            var updateDto = new UpdateItemDto
            {
                Id = item.Id,
                ProductId = productId,
                Quantity = 25
            };

            var response = await _client.PutAsJsonAsync(
                "/api/Item",
                updateDto);

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task Update_ShouldReturnNotFound_WhenItemDoesNotExist()
        {
            await AuthenticateAsync();

            var dto = new UpdateItemDto
            {
                Id = 99999,
                ProductId = 1,
                Quantity = 50
            };

            var response = await _client.PutAsJsonAsync(
                "/api/Item",
                dto);

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Delete_ShouldReturnOk()
        {
            await AuthenticateAsync();

            var productId = await CreateProductAsync();

            var createDto = new CreateItemDto
            {
                ProductId = productId,
                Quantity = 10
            };

            await _client.PostAsJsonAsync(
                "/api/Item",
                createDto);

            var items = await _client.GetFromJsonAsync<List<ItemDto>>(
                "/api/Item");

            var item = items!.Last();

            var response = await _client.DeleteAsync(
                $"/api/Item/{item.Id}");

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task Delete_ShouldReturnNotFound_WhenItemDoesNotExist()
        {
            await AuthenticateAsync();

            var response = await _client.DeleteAsync(
                "/api/Item/99999");

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task GetById_ShouldReturnOk_WhenItemExists()
        {
            await AuthenticateAsync();

            var productId = await CreateProductAsync();

            var createDto = new CreateItemDto
            {
                ProductId = productId,
                Quantity = 15
            };

            await _client.PostAsJsonAsync(
                "/api/Item",
                createDto);

            var items = await _client.GetFromJsonAsync<List<ItemDto>>(
                "/api/Item");

            var item = items!.Last();

            var response = await _client.GetAsync(
                $"/api/Item/{item.Id}");

            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var result = await response.Content
                .ReadFromJsonAsync<ItemDto>();

            result.Should().NotBeNull();
            result!.Id.Should().Be(item.Id);
            result.ProductId.Should().Be(item.ProductId);
            result.Quantity.Should().Be(item.Quantity);
        }
    }
}