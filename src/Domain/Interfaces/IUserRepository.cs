// src/Domain/Interfaces/IUserRepository.cs
using Multilevelteam.Platform.Domain.Entities;

namespace Multilevelteam.Platform.Domain.Interfaces
{
    public interface IUserRepository
    {
        Task<User> GetByIdAsync(Guid id);
        Task<User> GetByPhoneNumberAsync(string phoneNumber);
        Task<bool> ExistsByPhoneNumberAsync(string phoneNumber);
        Task<User> CreateAsync(User user);
        Task UpdateAsync(User user);
        Task<bool> SaveTelegramInfoAsync(string phoneNumber, string chatId);
        Task<User> GetByTelegramChatIdAsync(string chatId);
        Task<bool> DeleteAsync(Guid id);
        Task<TelegramUser> GetTelegramUserByPhoneNumberAsync(string phoneNumber);
        Task<TelegramUser> GetTelegramUserByChatIdAsync(string chatId);
        Task UpdateTelegramUserAsync(TelegramUser telegramUser);
    }
}
