using AutoMapper;
using BCrypt.Net;
using Microsoft.EntityFrameworkCore;
using ProductSphere.Application.DTOs.AuthDtos;
using ProductSphere.Application.Interfaces.IServices;
using ProductSphere.Domain.Entities;
using ProductSphere.Domain.Exceptions;
using ProductSphere.Infrastructure.Data;

namespace ProductSphere.Infrastructure.Identity
{
    public class AuthService : IAuthService
    {
        private readonly ApplicationDbContext _context;
        private readonly IJwtTokenService _jwtTokenService;
        private readonly IMapper _mapper;

        public AuthService(
            ApplicationDbContext context,
            IJwtTokenService jwtTokenService,
            IMapper mapper)
        {
            _context = context;
            _jwtTokenService = jwtTokenService;
            _mapper = mapper;
        }

        public async Task<AuthResponseDto> RegisterAsync(RegisterRequestDto request)
        {
            var existingUser = await _context.Users
                .FirstOrDefaultAsync(x => x.Email == request.Email);

            if (existingUser != null)
                throw new BadRequestException("User already exists.");

        var role = await _context.Roles
            .FirstOrDefaultAsync(x => x.Id == request.RoleId);

            if (role == null)
                throw new NotFoundException("Role not found.");

        var user = new User
        {
            Id = Guid.NewGuid(),
            UserName = request.UserName,
            NormalizedUserName = request.UserName.ToUpper(),
            Email = request.Email,
            NormalizedEmail = request.Email.ToUpper(),
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
            RoleId = request.RoleId,
            Role = role,
            IsActive = true,
            CreatedAtUtc = DateTime.UtcNow
        };

        var refreshToken = _jwtTokenService.GenerateRefreshToken();
        refreshToken.UserId = user.Id;

            await _context.Users.AddAsync(user);
        await _context.RefreshTokens.AddAsync(refreshToken);
        await _context.SaveChangesAsync();

            return new AuthResponseDto
            {
            AccessToken = _jwtTokenService.GenerateAccessToken(user),
                RefreshToken = refreshToken.Token,
                AccessTokenExpiresAtUtc = DateTime.UtcNow.AddMinutes(15),
                RefreshTokenExpiresAtUtc = refreshToken.ExpiresAtUtc,
                UserName = user.UserName,
                Email = user.Email,
                Role = role.Name
    };
}

public async Task<AuthResponseDto> LoginAsync(LoginRequestDto request)
{
    var user = await _context.Users
        .Include(x => x.Role)
        .FirstOrDefaultAsync(x => x.Email == request.Email);

    if (user == null)
        throw new BadRequestException("Invalid email or password.");

    if (!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
        throw new BadRequestException("Invalid email or password.");

    var refreshToken = _jwtTokenService.GenerateRefreshToken();
    refreshToken.UserId = user.Id;

    await _context.RefreshTokens.AddAsync(refreshToken);
    await _context.SaveChangesAsync();

    return new AuthResponseDto
    {
        AccessToken = _jwtTokenService.GenerateAccessToken(user),
        RefreshToken = refreshToken.Token,
        AccessTokenExpiresAtUtc = DateTime.UtcNow.AddMinutes(15),
        RefreshTokenExpiresAtUtc = refreshToken.ExpiresAtUtc,
        UserName = user.UserName,
        Email = user.Email,
        Role = user.Role.Name
    };
}

public async Task<AuthResponseDto> RefreshTokenAsync(RefreshTokenRequestDto request)
{
    var existingToken = await _context.RefreshTokens
        .Include(x => x.User)
        .ThenInclude(x => x.Role)
        .FirstOrDefaultAsync(x => x.Token == request.RefreshToken);

    if (existingToken == null)
        throw new BadRequestException("Invalid refresh token.");

    if (existingToken.IsRevoked)
        throw new BadRequestException("Refresh token has been revoked.");

    if (existingToken.ExpiresAtUtc <= DateTime.UtcNow)
        throw new BadRequestException("Refresh token has expired.");

    existingToken.RevokedAtUtc = DateTime.UtcNow;

    var newRefreshToken = _jwtTokenService.GenerateRefreshToken();
    newRefreshToken.UserId = existingToken.UserId;

    await _context.RefreshTokens.AddAsync(newRefreshToken);
    await _context.SaveChangesAsync();

    return new AuthResponseDto
    {
        AccessToken = _jwtTokenService.GenerateAccessToken(existingToken.User),
        RefreshToken = newRefreshToken.Token,
        AccessTokenExpiresAtUtc = DateTime.UtcNow.AddMinutes(15),
        RefreshTokenExpiresAtUtc = newRefreshToken.ExpiresAtUtc,
        UserName = existingToken.User.UserName,
        Email = existingToken.User.Email,
        Role = existingToken.User.Role.Name
    };
}
    }
}