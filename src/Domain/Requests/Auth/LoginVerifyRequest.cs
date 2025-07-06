using System.ComponentModel.DataAnnotations;

namespace Multilevelteam.Platform.API.DTOs.Auth
{
    public class LoginVerifyRequest
    {
        [Required]
        [Phone]
        public string PhoneNumber { get; set; }

        [Required]
        [StringLength(6, MinimumLength = 6)]
        public string Code { get; set; }

        [Required]
        [MinLength(6)]
        public string Password { get; set; }
    }
} 