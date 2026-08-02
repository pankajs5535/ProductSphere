using AutoMapper;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using ProductSphere.Application.DTOs.AuthDtos;
using ProductSphere.Application.Interfaces.IServices;
using ProductSphere.Domain.Entities;
using ProductSphere.Domain.Exceptions;
using ProductSphere.Infrastructure.Data;
using ProductSphere.Infrastructure.Identity;

namespace ProductSphere.Application.Tests;

public class AuthServiceTests
{
    private readonly ApplicationDbContext _context;
    private readonly Mock<IJwtTokenService> _jwtTokenServiceMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly AuthService _service;

    public AuthServiceTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        _context = new ApplicationDbContext(options);

        _jwtTokenServiceMock = new Mock<IJwtTokenService>();
        _mapperMock = new Mock<IMapper>();

        _service = new AuthService(
            _context,
            _jwtTokenServiceMock.Object,
            _mapperMock.Object);
    }

    [Fact]
    public async Task RegisterAsync_ShouldRegisterUserSuccessfully()
    {
        var roleId = Guid.NewGuid();

        var role = new Role
        {
            Id = roleId,
            Name = "Admin",
            NormalizedName = "ADMIN"
        };

        await _context.Roles.AddAsync(role);
        await _context.SaveChangesAsync();

        var request = new RegisterRequestDto
        {
            UserName = "admin",
            Email = "admin@test.com",
            Password = "Password@123",
            RoleId = roleId
        };

        var refreshToken = new RefreshToken
        {
            Id = Guid.NewGuid(),
            Token = "refresh-token",
            ExpiresAtUtc = DateTime.UtcNow.AddDays(7)
        };

        _jwtTokenServiceMock
            .Setup(x => x.GenerateRefreshToken())
            .Returns(refreshToken);

        _jwtTokenServiceMock
            .Setup(x => x.GenerateAccessToken(It.IsAny<User>()))
            .Returns("access-token");

        var result = await _service.RegisterAsync(request);

        result.Should().NotBeNull();
        result.AccessToken.Should().Be("access-token");
        result.RefreshToken.Should().Be("refresh-token");

        _context.Users.Count().Should().Be(1);
        _context.RefreshTokens.Count().Should().Be(1);
    }

    [Fact]
    public async Task RegisterAsync_ShouldThrowBadRequestException_WhenUserAlreadyExists()
    {
        await _context.Users.AddAsync(new User
        {
            Id = Guid.NewGuid(),
            UserName = "admin",
            NormalizedUserName = "ADMIN",
            Email = "admin@test.com",
            NormalizedEmail = "ADMIN@TEST.COM",
            PasswordHash = "hash",
            RoleId = Guid.NewGuid()
        });

        await _context.SaveChangesAsync();

        var request = new RegisterRequestDto
        {
            UserName = "admin",
            Email = "admin@test.com",
            Password = "Password@123",
            RoleId = Guid.NewGuid()
        };

        Func<Task> act = async () => await _service.RegisterAsync(request);

        await act.Should().ThrowAsync<BadRequestException>();
    }

    [Fact]
    public async Task RegisterAsync_ShouldThrowNotFoundException_WhenRoleDoesNotExist()
    {
        var request = new RegisterRequestDto
        {
            UserName = "admin",
            Email = "admin@test.com",
            Password = "Password@123",
            RoleId = Guid.NewGuid()
        };

        Func<Task> act = async () => await _service.RegisterAsync(request);

        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task LoginAsync_ShouldLoginSuccessfully()
    {
        var roleId = Guid.NewGuid();

        var role = new Role
        {
            Id = roleId,
            Name = "Admin",
            NormalizedName = "ADMIN"
        };

        var user = new User
        {
            Id = Guid.NewGuid(),
            UserName = "admin",
            NormalizedUserName = "ADMIN",
            Email = "admin@test.com",
            NormalizedEmail = "ADMIN@TEST.COM",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("Password@123"),
            RoleId = roleId,
            Role = role
        };

        await _context.Roles.AddAsync(role);
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();

        var refreshToken = new RefreshToken
        {
            Id = Guid.NewGuid(),
            Token = "refresh-token",
            ExpiresAtUtc = DateTime.UtcNow.AddDays(7)
        };

        _jwtTokenServiceMock
            .Setup(x => x.GenerateRefreshToken())
            .Returns(refreshToken);

        _jwtTokenServiceMock
            .Setup(x => x.GenerateAccessToken(It.IsAny<User>()))
            .Returns("access-token");

        var request = new LoginRequestDto
        {
            Email = "admin@test.com",
            Password = "Password@123"
        };

        var result = await _service.LoginAsync(request);

        result.Should().NotBeNull();
        result.AccessToken.Should().Be("access-token");
        result.RefreshToken.Should().Be("refresh-token");

        _context.RefreshTokens.Count().Should().Be(1);
    }

    [Fact]
    public async Task LoginAsync_ShouldThrowBadRequestException_WhenUserDoesNotExist()
    {
        var request = new LoginRequestDto
        {
            Email = "unknown@test.com",
            Password = "Password@123"
        };

        Func<Task> act = async () => await _service.LoginAsync(request);

        await act.Should().ThrowAsync<BadRequestException>();
    }

    [Fact]
    public async Task LoginAsync_ShouldThrowBadRequestException_WhenPasswordIsInvalid()
    {
        var roleId = Guid.NewGuid();

        var role = new Role
        {
            Id = roleId,
            Name = "Admin",
            NormalizedName = "ADMIN"
        };

        var user = new User
        {
            Id = Guid.NewGuid(),
            UserName = "admin",
            NormalizedUserName = "ADMIN",
            Email = "admin@test.com",
            NormalizedEmail = "ADMIN@TEST.COM",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("CorrectPassword"),
            RoleId = roleId,
            Role = role
        };

        await _context.Roles.AddAsync(role);
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();

        var request = new LoginRequestDto
        {
            Email = "admin@test.com",
            Password = "WrongPassword"
        };

        Func<Task> act = async () => await _service.LoginAsync(request);

        await act.Should().ThrowAsync<BadRequestException>();
    }
    [Fact]
    public async Task RefreshTokenAsync_ShouldReturnNewTokens_WhenTokenIsValid()
    {
        var roleId = Guid.NewGuid();

        var role = new Role
        {
            Id = roleId,
            Name = "Admin",
            NormalizedName = "ADMIN"
        };

        var user = new User
        {
            Id = Guid.NewGuid(),
            UserName = "admin",
            NormalizedUserName = "ADMIN",
            Email = "admin@test.com",
            NormalizedEmail = "ADMIN@TEST.COM",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("Password@123"),
            RoleId = roleId,
            Role = role
        };

        var oldToken = new RefreshToken
        {
            Id = Guid.NewGuid(),
            Token = "old-refresh-token",
            UserId = user.Id,
            User = user,
            CreatedAtUtc = DateTime.UtcNow,
            ExpiresAtUtc = DateTime.UtcNow.AddDays(7)
        };

        await _context.Roles.AddAsync(role);
        await _context.Users.AddAsync(user);
        await _context.RefreshTokens.AddAsync(oldToken);
        await _context.SaveChangesAsync();

        var newToken = new RefreshToken
        {
            Id = Guid.NewGuid(),
            Token = "new-refresh-token",
            ExpiresAtUtc = DateTime.UtcNow.AddDays(7)
        };

        _jwtTokenServiceMock
            .Setup(x => x.GenerateRefreshToken())
            .Returns(newToken);

        _jwtTokenServiceMock
            .Setup(x => x.GenerateAccessToken(It.IsAny<User>()))
            .Returns("new-access-token");

        var request = new RefreshTokenRequestDto
        {
            RefreshToken = "old-refresh-token"
        };

        var result = await _service.RefreshTokenAsync(request);

        result.Should().NotBeNull();
        result.AccessToken.Should().Be("new-access-token");
        result.RefreshToken.Should().Be("new-refresh-token");

        _context.RefreshTokens.Count().Should().Be(2);
    }

    [Fact]
    public async Task RefreshTokenAsync_ShouldThrowBadRequestException_WhenTokenDoesNotExist()
    {
        var request = new RefreshTokenRequestDto
        {
            RefreshToken = "invalid-token"
        };

        Func<Task> act = async () => await _service.RefreshTokenAsync(request);

        await act.Should().ThrowAsync<BadRequestException>();
    }

    [Fact]
    public async Task RefreshTokenAsync_ShouldThrowBadRequestException_WhenTokenIsRevoked()
    {
        var token = new RefreshToken
        {
            Id = Guid.NewGuid(),
            Token = "revoked-token",
            UserId = Guid.NewGuid(),
            RevokedAtUtc = DateTime.UtcNow,
            ExpiresAtUtc = DateTime.UtcNow.AddDays(7)
        };

        await _context.RefreshTokens.AddAsync(token);
        await _context.SaveChangesAsync();

        var request = new RefreshTokenRequestDto
        {
            RefreshToken = "revoked-token"
        };

        Func<Task> act = async () => await _service.RefreshTokenAsync(request);

        await act.Should().ThrowAsync<BadRequestException>();
    }

    [Fact]
    public async Task RefreshTokenAsync_ShouldThrowBadRequestException_WhenTokenIsExpired()
    {
        var token = new RefreshToken
        {
            Id = Guid.NewGuid(),
            Token = "expired-token",
            UserId = Guid.NewGuid(),
            ExpiresAtUtc = DateTime.UtcNow.AddDays(-1)
        };

        await _context.RefreshTokens.AddAsync(token);
        await _context.SaveChangesAsync();

        var request = new RefreshTokenRequestDto
        {
            RefreshToken = "expired-token"
        };

        Func<Task> act = async () => await _service.RefreshTokenAsync(request);

        await act.Should().ThrowAsync<BadRequestException>();
    }
}