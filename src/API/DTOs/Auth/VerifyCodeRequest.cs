// src/API/DTOs/Auth/VerifyCodeRequest.cs
using System.ComponentModel.DataAnnotations;
using Autotest.Platform.Domain.Enums;

namespace Autotest.Platform.API.DTOs.Auth
{
    public class VerifyCodeRequest
    {
        [Required]
        [Phone]
        public string PhoneNumber { get; set; }

        [Required]
        [StringLength(6, MinimumLength = 6)]
        public string Code { get; set; }

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