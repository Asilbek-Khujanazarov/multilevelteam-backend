// src/API/DTOs/Auth/RefreshTokenRequest.cs
using System.ComponentModel.DataAnnotations;

namespace Multilevelteam.Platform.API.DTOs.Auth
{
    public class RefreshTokenRequest
    {
        [Required]
        public string AccessToken { get; set; }

        [Required]
        public string RefreshToken { get; set; }
    }
}