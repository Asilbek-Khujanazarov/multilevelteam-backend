using System.Security.Claims;
using Multilevelteam.Platform.Domain.Entities;


namespace Multilevelteam.Platform.Domain.Interfaces
{
    public interface IJwtService
    {
        string GenerateAccessToken(User user);
        string GenerateRefreshToken();
        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
        DateTime GetRefreshTokenExpiryTime();
    }
}