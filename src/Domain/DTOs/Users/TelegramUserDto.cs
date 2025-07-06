namespace Multilevelteam.Platform.API.DTOs.Users
{
    public class TelegramUserDto
    {
        public string ChatId { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime LastInteractionAt { get; set; }
    }
}