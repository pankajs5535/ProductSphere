using ProductSphere.Domain.Entities;

namespace ProductSphere.Application.Interfaces.IServices
{
    public interface IJwtTokenService
    {
        string GenerateAccessToken(User user);

        RefreshToken GenerateRefreshToken();
    }
}