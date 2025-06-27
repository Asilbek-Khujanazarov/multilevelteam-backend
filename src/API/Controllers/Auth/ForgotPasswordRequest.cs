using System.ComponentModel.DataAnnotations;

namespace Autotest.Platform.API.DTOs.Auth
{
    public class ForgotPasswordRequest
    {
        [Required]
        [Phone]
        public string PhoneNumber { get; set; }
    }
} 