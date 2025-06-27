// src/API/DTOs/Auth/LoginRequest.cs
using System.ComponentModel.DataAnnotations;

namespace Autotest.Platform.API.DTOs.Auth
{
    public class LoginRequest
    {
        [Required]
        [Phone]
        public string PhoneNumber { get; set; }

        [Required]
        public string Password { get; set; }
    }
}