// src/Domain/Entities/TelegramUser.cs
using System;
using System.ComponentModel.DataAnnotations;

namespace Multilevelteam.Platform.Domain.Entities
{
    public class TelegramUser
    {
        public TelegramUser()
        {
            Id = Guid.NewGuid();
            CreatedAt = DateTime.UtcNow;
            LastInteractionAt = DateTime.UtcNow;
        }

        [Key]
        public Guid Id { get; set; }

        [Required]
        public string ChatId { get; set; }

        [Required]
        [Phone]
        [MaxLength(15)]
        public string PhoneNumber { get; set; }

        public DateTime CreatedAt { get; private set; }

        public DateTime LastInteractionAt { get; set; }

        // Navigation property
        public Guid? UserId { get; set; }
        public virtual User User { get; set; }
    }
}

