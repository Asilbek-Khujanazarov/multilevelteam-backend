using System.ComponentModel.DataAnnotations;

namespace Multilevelteam.Platform.API.DTOs.Auth
{
    public class ForgotPasswordRequest
    {
        [Required]
        [Phone]
        public string PhoneNumber { get; set; }
    }
} 