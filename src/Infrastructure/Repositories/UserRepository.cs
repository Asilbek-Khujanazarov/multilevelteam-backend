// src/Infrastructure/Repositories/UserRepository.cs
using Microsoft.EntityFrameworkCore;
using Multilevelteam.Platform.Domain.Entities;
using Multilevelteam.Platform.Domain.Interfaces;
using Multilevelteam.Platform.Infrastructure.Data;
using Microsoft.Extensions.Logging;

namespace Multilevelteam.Platform.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;
        private ILogger<UserRepository> _logger;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<User> GetByIdAsync(Guid id)
        {
            return await _context.Users
                .Include(u => u.VerificationCodes)
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<User> GetByPhoneNumberAsync(string phoneNumber)
        {
            return await _context.Users
                .Include(u => u.VerificationCodes)
                .FirstOrDefaultAsync(u => u.PhoneNumber == phoneNumber);
        }

        public async Task<bool> ExistsByPhoneNumberAsync(string phoneNumber)
        {
            return await _context.Users
                .AnyAsync(u => u.PhoneNumber == phoneNumber);
        }

        public async Task<User> CreateAsync(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task UpdateAsync(User user)
        {
            _context.Entry(user).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task<bool> SaveTelegramInfoAsync(string phoneNumber, string chatId)
        {
            try
            {
                // Telefon raqamni normalizatsiya qilish
                phoneNumber = NormalizePhoneNumber(phoneNumber);

                // Mavjud TelegramUser ni tekshirish
                var telegramUser = await _context.TelegramUsers
                    .FirstOrDefaultAsync(t => t.PhoneNumber == phoneNumber || t.ChatId == chatId);

                if (telegramUser == null)
                {
                    // Yangi TelegramUser yaratish
                    telegramUser = new TelegramUser
                    {
                        PhoneNumber = phoneNumber,
                        ChatId = chatId,
                        LastInteractionAt = DateTime.UtcNow
                    };
                    _context.TelegramUsers.Add(telegramUser);
                }
                else
                {
                    // Mavjud TelegramUser ni yangilash
                    telegramUser.PhoneNumber = phoneNumber;
                    telegramUser.ChatId = chatId;
                    telegramUser.LastInteractionAt = DateTime.UtcNow;
                }

                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger = LoggerFactory.Create(builder => builder.AddConsole())
                    .CreateLogger<UserRepository>();
                _logger.LogError(ex, "Error saving telegram info for phone {Phone}", phoneNumber);
                return false;
            }
        }

        public async Task<TelegramUser> GetTelegramUserByPhoneNumberAsync(string phoneNumber)
        {
            return await _context.TelegramUsers
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.PhoneNumber == phoneNumber);
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

        public async Task<User> GetByTelegramChatIdAsync(string chatId)
        {
            return await _context.Users
                .Include(u => u.VerificationCodes)
                .FirstOrDefaultAsync(u => u.TelegramUser.ChatId == chatId);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var user = await GetByIdAsync(id);
            if (user != null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task UpdateTelegramUserAsync(TelegramUser telegramUser)
        {
            _context.TelegramUsers.Update(telegramUser);
            await _context.SaveChangesAsync();
        }

        public async Task<TelegramUser> GetTelegramUserByChatIdAsync(string chatId)
        {
            return await _context.TelegramUsers
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.ChatId == chatId);
        }
    }
}