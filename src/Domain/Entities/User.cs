// src/Domain/Entities/User.cs
using System;
using System.ComponentModel.DataAnnotations;
using Autotest.Platform.Domain.Enums;

namespace Autotest.Platform.Domain.Entities
{
    public class User
    {
        public Guid Id { get; set; } = Guid.NewGuid(); 

        [Required]
        [Phone]
        [MaxLength(15)]
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
        public string PasswordHash { get; set; }

        public bool IsVerified { get; set; }

        public DateTime RegisteredDate { get; set; } = DateTime.UtcNow;

        public string TelegramChatId { get; set; }

        public DateTime? LastLoginDate { get; set; }

        public string RefreshToken { get; set; }

        public DateTime? RefreshTokenExpiryTime { get; set; }

        // Navigation properties
        public virtual ICollection<VerificationCode> VerificationCodes { get; set; }
    }
}