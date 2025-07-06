namespace Multilevelteam.Platform.API.DTOs.Users
{
    public class UserMeDto
    {
        // public Guid Id { get; set; }
        public string PhoneNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Role { get; set; }
        public string? AvatarUrl { get; set; }
        public string? AvatarPublicId { get; set; }
        public bool IsVerified { get; set; }
        public DateTime RegisteredDate { get; set; }
        public DateTime? LastLoginDate { get; set; }
        public TelegramUserDto TelegramUser { get; set; }
    }
}