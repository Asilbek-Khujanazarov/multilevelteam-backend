// src/API/DTOs/Auth/VerifyCodeRequest.cs
using System.ComponentModel.DataAnnotations;

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
    }
}