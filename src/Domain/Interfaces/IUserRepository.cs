using Autotest.Platform.Domain.Entities;

namespace Autotest.Platform.Domain.Interfaces
{
    public interface IUserRepository
    {
        Task<User> GetByIdAsync(Guid id);
        Task<User> GetByPhoneNumberAsync(string phoneNumber);
        Task<bool> ExistsByPhoneNumberAsync(string phoneNumber);
        Task<User> CreateAsync(User user);
        Task UpdateAsync(User user);
        Task<bool> SaveTelegramInfoAsync(string phoneNumber, string chatId);
        Task<bool> DeleteAsync(Guid id);
        Task<User> GetByTelegramChatIdAsync(string chatId); // Yangi metod
    }
}