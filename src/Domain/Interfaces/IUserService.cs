
using Autotest.Platform.API.DTOs.Users;

namespace Autotest.Platform.Domain.Interfaces
{
    public interface IUserService
    {
        Task<UserMeDto> GetCurrentUserAsync(Guid userId);
        Task<UserDto> GetUserByIdAsync(Guid id);
        Task<UserDto> GetUserByPhoneNumberAsync(string phoneNumber);
        Task<UserDto> GetUserByTelegramChatIdAsync(string chatId);
        Task<TelegramUserDto> GetTelegramUserByPhoneNumberAsync(string phoneNumber);
        Task<bool> DeleteUserAsync(Guid id);
    }
}