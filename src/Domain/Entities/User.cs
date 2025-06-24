// src/Domain/Entities/User.cs
using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Autotest.Platform.Domain.Enums;

namespace Autotest.Platform.Domain.Entities
{
    public class User
    {
        public User()
        {
            Id = Guid.NewGuid();
            IsVerified = false;
            RefreshToken = string.Empty; // Initialize with empty string
        }

        [Key]
        public Guid Id { get; set; }

        [Required]
        [Phone]
        [MaxLength(15)]
        public string PhoneNumber { get; set; }

        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(50)]
        public string LastName { get; set; }

        [Required]
        public Gender Gender { get; set; }
        [JsonIgnore]
        public string Role { get; set; } = "User";

        [Required]
        public string PasswordHash { get; set; }
        public string TelegramBotchatId { get; set; }
        public bool IsVerified { get; set; }
        public string? AvatarUrl { get; set; }
        public string? AvatarPublicId { get; set; }
        public DateTime RegisteredDate { get; set; } = DateTime.UtcNow;

        public DateTime? LastLoginDate { get; set; }

        public string RefreshToken { get; set; }

        public DateTime? RefreshTokenExpiryTime { get; set; }

        // Navigation properties
        public virtual TelegramUser TelegramUser { get; set; }
        public virtual ICollection<VerificationCode> VerificationCodes { get; set; }
    }
}