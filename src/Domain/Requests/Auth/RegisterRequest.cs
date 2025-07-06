// src/API/DTOs/Auth/RegisterRequest.cs
using System.ComponentModel.DataAnnotations;

namespace Multilevelteam.Platform.API.DTOs.Auth
{
    public class RegisterRequest
    {
        [Required]
        [Phone]
        public string PhoneNumber { get; set; }
    }
}




