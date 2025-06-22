using System.Security.Claims;
using Autotest.Platform.Domain.Entities;


namespace Autotest.Platform.Domain.Interfaces
{
    public interface IJwtService
    {
        string GenerateAccessToken(User user);
        string GenerateRefreshToken();
        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
        DateTime GetRefreshTokenExpiryTime();
    }
}