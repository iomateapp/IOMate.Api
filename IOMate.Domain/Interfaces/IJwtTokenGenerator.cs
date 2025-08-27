using IOMate.Domain.Entities;
using System.Security.Claims;

namespace IOMate.Domain.Interfaces
{
    public interface IJwtTokenGenerator
    {
        string GenerateToken(User user);
        string GenerateRefreshToken(User user);
        ClaimsPrincipal? GetPrincipalFromExpiredToken(string token, bool isRefreshToken = false);
    }
}
