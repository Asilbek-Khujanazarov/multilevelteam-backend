// src/Domain/Entities/TelegramUser.cs
using System.ComponentModel.DataAnnotations;

namespace Autotest.Platform.Domain.Entities
{
    public class TelegramUser
    {
        public TelegramUser()
        {
            Id = Guid.NewGuid();
            CreatedAt = DateTime.UtcNow;
        }

        [Key]
        public Guid Id { get; set; }

        [Required]
        public long ChatId { get; set; }

        public string Username { get; set; }

        [Required]
        public string FirstName { get; set; }

        public string LastName { get; set; }

        [Phone]
        public string PhoneNumber { get; set; }

        public bool IsPhoneNumberConfirmed { get; set; }

        public DateTime CreatedAt { get; private set; }

        public DateTime? LastInteractionAt { get; set; }

        // Navigation property
        public Guid? UserId { get; set; }
        public virtual User User { get; set; }
    }
}

