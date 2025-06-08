// src/Domain/Entities/VerificationCode.cs
using System;
using System.ComponentModel.DataAnnotations;
using Autotest.Platform.Domain.Enums;

namespace Autotest.Platform.Domain.Entities
{
    public class VerificationCode
    {
        public VerificationCode()
        {
            Id = Guid.NewGuid();
            CreatedAt = DateTime.UtcNow;
        }

        [Key]
        public Guid Id { get; set; }

        [Required]
        [Phone]
        [MaxLength(15)]
        public string PhoneNumber { get; set; }

        [Required]
        [StringLength(6)]
        public string Code { get; set; }

        public DateTime CreatedAt { get; private set; }

        public DateTime ExpiryTime { get; set; }

        public bool IsUsed { get; set; }

        public VerificationPurpose Purpose { get; set; }

        // Navigation property
        public Guid? UserId { get; set; }
        public virtual User User { get; set; }
    }
}