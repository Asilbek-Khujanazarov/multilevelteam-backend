using Microsoft.EntityFrameworkCore;
using Autotest.Platform.Domain.Entities;
using Autotest.Platform.Domain.Interfaces;
using Autotest.Platform.Infrastructure.Data;

namespace Autotest.Platform.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;
        private readonly ILogger<UserRepository> _logger;

        public UserRepository(AppDbContext context, ILogger<UserRepository> logger)
        {
            _context = context;
            _logger = logger;
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
                var user = await GetByPhoneNumberAsync(phoneNumber);

                if (user == null)
                {
                    // Yangi foydalanuvchi yaratamiz
                    user = new User
                    {
                        Id = Guid.NewGuid(),
                        PhoneNumber = phoneNumber,
                        TelegramChatId = chatId,
                        RegisteredDate = DateTime.UtcNow,
                        IsVerified = false
                    };
                    await CreateAsync(user);
                }
                else
                {
                    // Mavjud foydalanuvchini yangilaymiz
                    user.TelegramChatId = chatId;
                    await UpdateAsync(user);
                }

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving telegram info for phone {Phone}", phoneNumber);
                return false;
            }
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

        public async Task<User> GetByTelegramChatIdAsync(string chatId)
        {
            return await _context.Users
                .Include(u => u.VerificationCodes)
                .FirstOrDefaultAsync(u => u.TelegramChatId == chatId);
        }
    }
}