// src/API/DTOs/Auth/RegisterRequest.cs
using System.ComponentModel.DataAnnotations;
using Autotest.Platform.Domain.Enums;

namespace Autotest.Platform.API.DTOs.Auth
{
    public class RegisterRequest
    {
        [Required]
        [Phone]
        public string PhoneNumber { get; set; }

        [Required]
        [MinLength(2)]
        [MaxLength(50)]
        public string FirstName { get; set; }

        [Required]
        [MinLength(2)]
        [MaxLength(50)]
        public string LastName { get; set; }

        [Required]
        public Gender Gender { get; set; }

        [Required]
        [MinLength(6)]
        public string Password { get; set; }
    }
}




