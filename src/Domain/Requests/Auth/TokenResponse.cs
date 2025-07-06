
// src/API/DTOs/Auth/TokenResponse.cs
namespace Multilevelteam.Platform.API.DTOs.Auth
{
    public class TokenResponse
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public DateTime AccessTokenExpiration { get; set; }
        public DateTime RefreshTokenExpiration { get; set; }
    }
}
