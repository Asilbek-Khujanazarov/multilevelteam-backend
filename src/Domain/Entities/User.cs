using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Multilevelteam.Platform.Domain.Enums;

namespace Multilevelteam.Platform.Domain.Entities
{
    public class User
    {
        public User()
        {
            CreatedCourses = new List<Course>();
            AllowedCourses = new List<CourseAllowedUser>();
            PurchasedCourses = new List<CoursePurchasedUser>();
            VerificationCodes = new List<VerificationCode>();
        }

        [Key]
        public Guid Id { get; set; }

        [Required, Phone, MaxLength(15)]
        public string PhoneNumber { get; set; }

        [Required, MaxLength(50)]
        public string FirstName { get; set; }

        [Required, MaxLength(50)]
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

        // Navigation
        public virtual TelegramUser TelegramUser { get; set; }
        public virtual ICollection<VerificationCode> VerificationCodes { get; set; }

        // Teacher yaratgan kurslar
        public virtual ICollection<Course> CreatedCourses { get; set; }

        // Private kurslarga ruxsati bor userlar
        public virtual ICollection<CourseAllowedUser> AllowedCourses { get; set; }

        // Sotib olingan public kurslar
        public virtual ICollection<CoursePurchasedUser> PurchasedCourses { get; set; }
    }
}