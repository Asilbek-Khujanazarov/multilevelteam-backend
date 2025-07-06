// src/Infrastructure/Services/TelegramBotService.cs

using Telegram.Bot.Types;

namespace Multilevelteam.Platform.Domain.Interfaces
{
    public interface ITelegramBotService
    {
        Task<bool> IsUserSubscribedAsync(long chatId);
        Task<bool> SendVerificationCodeAsync(long chatId, string code);
        Task<bool> SendWelcomeMessageAsync(long chatId, string firstName);
        Task<bool> RequestPhoneNumberAsync(long chatId);
        Task HandleUpdateAsync(Update update);
    }
}