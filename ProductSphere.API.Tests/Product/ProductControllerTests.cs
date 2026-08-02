using FluentAssertions;
using ProductSphere.Application.DTOs.AuthDtos;
using ProductSphere.Application.DTOs.ProductDtos;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace ProductSphere.API.Tests
{
    public class ProductControllerTests
        : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _client;

        public ProductControllerTests(CustomWebApplicationFactory factory)
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

            var response = await _client.PostAsJsonAsync(
                "/api/Auth/login",
                login);

            response.EnsureSuccessStatusCode();

            var auth = await response.Content
                .ReadFromJsonAsync<AuthResponseDto>();

            _client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue(
                    "Bearer",
                    auth!.AccessToken);
        }

        [Fact]
        public async Task GetAll_ShouldReturnOk()
        {
            await AuthenticateAsync();

            var response = await _client.GetAsync("/api/Product");

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task GetById_ShouldReturnNotFound_WhenProductDoesNotExist()
        {
            await AuthenticateAsync();

            var response = await _client.GetAsync("/api/Product/99999");

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Create_ShouldReturnOk()
        {
            await AuthenticateAsync();

            var dto = new CreateProductDto
            {
                ProductName = "Laptop",
                CreatedBy = "IntegrationTest"
            };

            var response = await _client.PostAsJsonAsync(
                "/api/Product",
                dto);

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task Create_ShouldReturnBadRequest_WhenModelIsInvalid()
        {
            await AuthenticateAsync();

            var dto = new CreateProductDto();

            var response = await _client.PostAsJsonAsync(
                "/api/Product",
                dto);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Update_ShouldReturnOk()
        {
            await AuthenticateAsync();

            var createDto = new CreateProductDto
            {
                ProductName = "Laptop",
                CreatedBy = "IntegrationTest"
            };

            var createResponse = await _client.PostAsJsonAsync(
                "/api/Product",
                createDto);

            createResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            var products = await _client.GetFromJsonAsync<List<ProductDto>>(
                "/api/Product");

            var product = products!.Last();

            var updateDto = new UpdateProductDto
            {
                Id = product.Id,
                ProductName = "Updated Laptop"
            };

            var response = await _client.PutAsJsonAsync(
                "/api/Product",
                updateDto);

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task Update_ShouldReturnNotFound_WhenProductDoesNotExist()
        {
            await AuthenticateAsync();

            var dto = new UpdateProductDto
            {
                Id = 99999,
                ProductName = "Test"
            };

            var response = await _client.PutAsJsonAsync(
                "/api/Product",
                dto);

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Delete_ShouldReturnOk()
        {
            await AuthenticateAsync();

            var createDto = new CreateProductDto
            {
                ProductName = "Mouse",
                CreatedBy = "IntegrationTest"
            };

            await _client.PostAsJsonAsync(
                "/api/Product",
                createDto);

            var products = await _client.GetFromJsonAsync<List<ProductDto>>(
                "/api/Product");

            var product = products!.Last();

            var response = await _client.DeleteAsync(
                $"/api/Product/{product.Id}");

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
        [Fact]
        public async Task Delete_ShouldReturnNotFound_WhenProductDoesNotExist()
        {
            await AuthenticateAsync();

            var response = await _client.DeleteAsync(
                "/api/Product/99999");

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task GetById_ShouldReturnOk_WhenProductExists()
        {
            await AuthenticateAsync();

            var createDto = new CreateProductDto
            {
                ProductName = "Keyboard",
                CreatedBy = "IntegrationTest"
            };

            await _client.PostAsJsonAsync(
                "/api/Product",
                createDto);

            var products = await _client.GetFromJsonAsync<List<ProductDto>>(
                "/api/Product");

            var product = products!.Last();

            var response = await _client.GetAsync(
                $"/api/Product/{product.Id}");

            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var result = await response.Content
                .ReadFromJsonAsync<ProductDto>();

            result.Should().NotBeNull();
            result!.Id.Should().Be(product.Id);
            result.ProductName.Should().Be(product.ProductName);
        }
    }
}