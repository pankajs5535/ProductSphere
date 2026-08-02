using FluentAssertions;
using ProductSphere.Application.DTOs.AuthDtos;
using System.Net;
using System.Net.Http.Json;

namespace ProductSphere.API.Tests
{
    public class AuthControllerTests
        : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _client;

        public AuthControllerTests(CustomWebApplicationFactory factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task Register_ShouldReturnOk()
        {
            var request = new RegisterRequestDto
            {
                UserName = "admin",
                Email = $"admin{Guid.NewGuid()}@test.com",
                Password = "Password@123",
                ConfirmPassword = "Password@123",
                RoleId = Guid.Parse("11111111-1111-1111-1111-111111111111")
            };

            var response = await _client.PostAsJsonAsync(
                "/api/Auth/register",
                request);

            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var auth = await response.Content.ReadFromJsonAsync<AuthResponseDto>();

            auth.Should().NotBeNull();
            auth!.AccessToken.Should().NotBeNullOrWhiteSpace();
            auth.RefreshToken.Should().NotBeNullOrWhiteSpace();
            auth.UserName.Should().Be(request.UserName);
            auth.Email.Should().Be(request.Email);
            auth.Role.Should().Be("Admin");
        }

        [Fact]
        public async Task Register_ShouldReturnBadRequest_WhenModelIsInvalid()
        {
            var request = new RegisterRequestDto
            {
                UserName = "",
                Email = "",
                Password = "",
                ConfirmPassword = "",
                RoleId = Guid.Empty
            };

            var response = await _client.PostAsJsonAsync(
                "/api/Auth/register",
                request);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Login_ShouldReturnOk_WhenCredentialsAreValid()
        {
            var register = new RegisterRequestDto
            {
                UserName = "admin",
                Email = $"admin{Guid.NewGuid()}@test.com",
                Password = "Password@123",
                ConfirmPassword = "Password@123",
                RoleId = Guid.Parse("11111111-1111-1111-1111-111111111111")
            };

            await _client.PostAsJsonAsync(
                "/api/Auth/register",
                register);

            var login = new LoginRequestDto
            {
                Email = register.Email,
                Password = register.Password
            };

            var response = await _client.PostAsJsonAsync(
                "/api/Auth/login",
                login);

            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var auth = await response.Content
                .ReadFromJsonAsync<AuthResponseDto>();

            auth.Should().NotBeNull();
            auth!.AccessToken.Should().NotBeNullOrWhiteSpace();
            auth.RefreshToken.Should().NotBeNullOrWhiteSpace();
            auth.UserName.Should().Be(register.UserName);
            auth.Email.Should().Be(register.Email);
            auth.Role.Should().Be("Admin");
        }


        [Fact]
        public async Task Login_ShouldReturnBadRequest_WhenPasswordIsInvalid()
        {
            var request = new LoginRequestDto
            {
                Email = "admin@test.com",
                Password = "WrongPassword"
            };

            var response = await _client.PostAsJsonAsync(
                "/api/Auth/login",
                request);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Login_ShouldReturnBadRequest_WhenUserDoesNotExist()
        {
            var request = new LoginRequestDto
            {
                Email = "nouser@test.com",
                Password = "Password@123"
            };

            var response = await _client.PostAsJsonAsync(
                "/api/Auth/login",
                request);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task RefreshToken_ShouldReturnOk()
        {
            var register = new RegisterRequestDto
            {
                UserName = "admin",
                Email = $"admin{Guid.NewGuid()}@test.com",
                Password = "Password@123",
                ConfirmPassword = "Password@123",
                RoleId = Guid.Parse("11111111-1111-1111-1111-111111111111")
            };

            await _client.PostAsJsonAsync(
                "/api/Auth/register",
                register);

            var login = new LoginRequestDto
            {
                Email = register.Email,
                Password = register.Password
            };

            var loginResponse = await _client.PostAsJsonAsync(
                "/api/Auth/login",
                login);

            loginResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            var auth = await loginResponse.Content
                .ReadFromJsonAsync<AuthResponseDto>();

            auth.Should().NotBeNull();

            var refresh = new RefreshTokenRequestDto
            {
                RefreshToken = auth!.RefreshToken
            };

            var response = await _client.PostAsJsonAsync(
                "/api/Auth/refresh-token",
                refresh);

            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var refreshResponse = await response.Content
                .ReadFromJsonAsync<AuthResponseDto>();

            refreshResponse.Should().NotBeNull();
            refreshResponse!.AccessToken.Should().NotBeNullOrWhiteSpace();
            refreshResponse.RefreshToken.Should().NotBeNullOrWhiteSpace();
        }


        [Fact]
        public async Task RefreshToken_ShouldReturnBadRequest_WhenTokenIsInvalid()
        {
            var request = new RefreshTokenRequestDto
            {
                RefreshToken = "invalid-refresh-token"
            };

            var response = await _client.PostAsJsonAsync(
                "/api/Auth/refresh-token",
                request);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Register_ShouldReturnBadRequest_WhenUserAlreadyExists()
        {
            var request = new RegisterRequestDto
            {
                UserName = "admin",
                Email = $"admin{Guid.NewGuid()}@test.com",
                Password = "Password@123",
                ConfirmPassword = "Password@123",
                // RoleId = Guid.NewGuid()
                RoleId = Guid.Parse("11111111-1111-1111-1111-111111111111")
            };

            await _client.PostAsJsonAsync(
                "/api/Auth/register",
                request);

            var response = await _client.PostAsJsonAsync(
                "/api/Auth/register",
                request);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
    }
}