// src/Infrastructure/Services/TelegramBotService.cs
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using Microsoft.Extensions.Options;
using Autotest.Platform.Infrastructure.Configuration;
using Autotest.Platform.Domain.Interfaces;
using Telegram.Bot.Types.Enums;

namespace Autotest.Platform.Infrastructure.Services
{
    public interface ITelegramBotService
    {
        Task<bool> IsUserSubscribedAsync(long chatId);
        Task<bool> SendVerificationCodeAsync(long chatId, string code);
        Task<bool> SendWelcomeMessageAsync(long chatId, string firstName);
        Task<bool> RequestPhoneNumberAsync(long chatId);
        Task HandleUpdateAsync(Update update);
    }

    public class TelegramBotService : ITelegramBotService
    {
        private readonly ITelegramBotClient _botClient;
        private readonly IUserRepository _userRepository;
        private readonly ILogger<TelegramBotService> _logger;
        private readonly TelegramBotConfiguration _config;

        public TelegramBotService(
            IOptions<TelegramBotConfiguration> config,
            IUserRepository userRepository,
            ILogger<TelegramBotService> logger)
        {
            _config = config.Value;
            _userRepository = userRepository;
            _logger = logger;
            _botClient = new TelegramBotClient(_config.Token);
        }

        public async Task<bool> IsUserSubscribedAsync(long chatId)
        {
            try
            {
                var chatMember = await _botClient.GetChatMemberAsync(
                    chatId: chatId,
                    userId: chatId
                );
                return chatMember.Status != ChatMemberStatus.Left &&
                       chatMember.Status != ChatMemberStatus.Kicked;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking user subscription for chat {ChatId}", chatId);
                return false;
            }
        }

        public async Task<bool> SendVerificationCodeAsync(long chatId, string code)
        {
            try
            {
                var message = $"🔐 Sizning tasdiqlash kodingiz: *{code}*\n\n" +
                            "Iltimos, bu kodni hech kim bilan bo'lishmang!";

                await _botClient.SendTextMessageAsync(
                    chatId: chatId,
                    text: message,
                    parseMode: ParseMode.Markdown
                );
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending verification code to chat {ChatId}", chatId);
                return false;
            }
        }

        public async Task<bool> SendWelcomeMessageAsync(long chatId, string firstName)
        {
            try
            {
                var message = $"Assalomu alaykum, *{firstName}*!\n\n" +
                            "AutoTest.uz platformasining rasmiy botiga xush kelibsiz. 🚗\n\n" +
                            "Platformada ro'yxatdan o'tish va tizimga kirish uchun " +
                            "telefon raqamingizni ulashishingiz kerak bo'ladi.\n\n" +
                            "Telefon raqamingizni ulashish uchun quyidagi tugmani bosing 👇";

                var keyboard = new ReplyKeyboardMarkup(
                    new[]
                    {
                        new[]
                        {
                            KeyboardButton.WithRequestContact("📱 Telefon raqamni ulashish")
                        }
                    }
                )
                {
                    ResizeKeyboard = true,
                    OneTimeKeyboard = true
                };

                await _botClient.SendTextMessageAsync(
                    chatId: chatId,
                    text: message,
                    parseMode: ParseMode.Markdown,
                    replyMarkup: keyboard
                );
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending welcome message to chat {ChatId}", chatId);
                return false;
            }
        }

        public async Task<bool> RequestPhoneNumberAsync(long chatId)
        {
            try
            {
                var message = "Iltimos, telefon raqamingizni ulashing 📱\n\n" +
                            "Bu raqam orqali siz platformada ro'yxatdan o'tasiz " +
                            "va tizimga kirishingiz mumkin bo'ladi.";

                var keyboard = new ReplyKeyboardMarkup(
                    new[]
                    {
                        new[]
                        {
                            KeyboardButton.WithRequestContact("📱 Telefon raqamni ulashish")
                        }
                    }
                )
                {
                    ResizeKeyboard = true,
                    OneTimeKeyboard = true
                };

                await _botClient.SendTextMessageAsync(
                    chatId: chatId,
                    text: message,
                    replyMarkup: keyboard
                );
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error requesting phone number from chat {ChatId}", chatId);
                return false;
            }
        }

        public async Task HandleUpdateAsync(Update update)
        {
            try
            {
                if (update.Message is not { } message)
                    return;

                switch (message.Text)
                {
                    case "/start":
                        await HandleStartCommandAsync(message);
                        break;
                    case "/help":
                        await HandleHelpCommandAsync(message);
                        break;
                    case "/profile":
                        await HandleProfileCommandAsync(message);
                        break;
                    default:
                        if (message.Contact is not null)
                        {
                            await HandleContactMessageAsync(message);
                        }
                        else
                        {
                            await HandleUnknownCommandAsync(message);
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error handling telegram update");
            }
        }

        private async Task HandleStartCommandAsync(Message message)
        {
            await SendWelcomeMessageAsync(message.Chat.Id, message.From.FirstName);
        }

        private async Task HandleHelpCommandAsync(Message message)
        {
            var helpMessage =
                "*AutoTest.uz Bot Yordam*\n\n" +
                "🤖 Bot buyruqlari:\n\n" +
                "• /start - Botni qayta ishga tushirish\n" +
                "• /help - Ushbu yordam xabarini ko'rsatish\n" +
                "• /profile - Profil ma'lumotlarini ko'rish\n\n" +
                "📱 Telefon raqamni ulash:\n" +
                "1. \"Telefon raqamni ulashish\" tugmasini bosing\n" +
                "2. Raqamni ulashishga ruxsat bering\n\n" +
                "🔐 Tasdiqlash kodlari:\n" +
                "• Ro'yxatdan o'tish yoki tizimga kirishda\n" +
                "• Bot orqali yuboriladi\n" +
                "• 5 daqiqa davomida amal qiladi\n\n" +
                "❓ Qo'shimcha savollar bo'lsa:\n" +
                "• support@autotest.uz";

            await _botClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: helpMessage,
                parseMode: ParseMode.Markdown
            );
        }

        private async Task HandleProfileCommandAsync(Message message)
        {
            try
            {
                if (message?.Chat == null)
                {
                    _logger.LogWarning("Message or chat is null");
                    return;
                }

                var user = await _userRepository.GetByTelegramChatIdAsync(message.Chat.Id.ToString());

                if (user == null)
                {
                    await RequestPhoneNumberAsync(message.Chat.Id);
                    return;
                }

                var profileMessage =
                    "*Sizning ma'lumotlaringiz:*\n\n" +
                    $"📱 Telefon: {user.PhoneNumber}\n" +
                    $"👤 Ism: {message.From?.FirstName ?? "Ko'rsatilmagan"}\n" +
                    $"✅ Status: {(user.IsVerified ? "Tasdiqlangan" : "Tasdiqlanmagan")}\n\n" +
                    "Platformaga kirish uchun: https://autotest.uz";

                await _botClient.SendTextMessageAsync(
                    chatId: message.Chat.Id,
                    text: profileMessage,
                    parseMode: ParseMode.Markdown,
                    cancellationToken: CancellationToken.None
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in HandleProfileCommandAsync");

                await _botClient.SendTextMessageAsync(
                    chatId: message.Chat.Id,
                    text: "🚫 Profil ma'lumotlarini olishda xatolik yuz berdi.",
                    cancellationToken: CancellationToken.None
                );
            }
        }

        private async Task HandleContactMessageAsync(Message message)
        {
            try
            {
                if (message?.Contact == null)
                {
                    _logger.LogWarning("Contact information is missing");
                    await RequestPhoneNumberAsync(message.Chat.Id);
                    return;
                }

                var phoneNumber = NormalizePhoneNumber(message.Contact.PhoneNumber);
                _logger.LogInformation("Processing contact for phone number: {Phone}", phoneNumber);

                var success = await _userRepository.SaveTelegramInfoAsync(
                    phoneNumber,
                    message.Chat.Id.ToString()
                );

                ReplyKeyboardMarkup keyboard;
                string responseMessage;

                if (success)
                {
                    keyboard = new ReplyKeyboardMarkup(
                        new[]
                        {
                    new[] { new KeyboardButton("/profile") },
                    new[] { new KeyboardButton("/help") }
                        }
                    )
                    {
                        ResizeKeyboard = true
                    };

                    responseMessage =
                        "✅ Telefon raqamingiz muvaffaqiyatli saqlandi.\n\n" +
                        "Endi siz platformada ro'yxatdan o'tishingiz yoki tizimga kirishingiz mumkin.\n\n" +
                        "Platformaga o'tish uchun: https://autotest.uz";
                }
                else
                {
                    keyboard = new ReplyKeyboardMarkup(
                        new[]
                        {
                    new[]
                    {
                        KeyboardButton.WithRequestContact("📱 Telefon raqamni qayta ulash")
                    }
                        }
                    )
                    {
                        ResizeKeyboard = true
                    };

                    responseMessage =
                        "❌ Xatolik yuz berdi.\n" +
                        "Iltimos, telefon raqamingizni qayta ulashga urinib ko'ring.";
                }

                await _botClient.SendTextMessageAsync(
                    chatId: message.Chat.Id,
                    text: responseMessage,
                    replyMarkup: keyboard,
                    parseMode: ParseMode.Markdown,
                    cancellationToken: CancellationToken.None
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in HandleContactMessageAsync");

                await _botClient.SendTextMessageAsync(
                    chatId: message.Chat.Id,
                    text: "🚫 Tizimda xatolik yuz berdi. Iltimos, keyinroq qayta urinib ko'ring.",
                    cancellationToken: CancellationToken.None
                );
            }
        }

        private async Task HandleUnknownCommandAsync(Message message)
        {
            await _botClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: "❌ Noto'g'ri buyruq.\n\nMavjud buyruqlarni ko'rish uchun /help ni bosing."
            );
        }

        private string NormalizePhoneNumber(string phoneNumber)
        {
            // Telefon raqamdan barcha bo'sh joylar va maxsus belgilarni olib tashlash
            phoneNumber = new string(phoneNumber.Where(char.IsDigit).ToArray());

            // Agar raqam 998 bilan boshlanmasa, qo'shish
            if (phoneNumber.Length == 9)
            {
                phoneNumber = "998" + phoneNumber;
            }
            // Agar raqam 8 bilan boshlansa, 998 bilan almashtirish
            else if (phoneNumber.StartsWith("8") && phoneNumber.Length == 10)
            {
                phoneNumber = "998" + phoneNumber.Substring(1);
            }

            return phoneNumber;
        }
    }
}