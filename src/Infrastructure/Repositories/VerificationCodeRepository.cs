// src/Infrastructure/Repositories/VerificationCodeRepository.cs
using Microsoft.EntityFrameworkCore;
using Autotest.Platform.Domain.Entities;
using Autotest.Platform.Domain.Interfaces;
using Autotest.Platform.Domain.Enums;
using Autotest.Platform.Infrastructure.Data;

namespace Autotest.Platform.Infrastructure.Repositories
{
    public class VerificationCodeRepository : IVerificationCodeRepository
    {
        private readonly AppDbContext _context;

        public VerificationCodeRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<VerificationCode> CreateAsync(VerificationCode code)
        {
            _context.VerificationCodes.Add(code);
            await _context.SaveChangesAsync();
            return code;
        }

        public async Task<VerificationCode> GetByIdAsync(Guid id)
        {
            return await _context.VerificationCodes
                .Include(v => v.User)
                .FirstOrDefaultAsync(v => v.Id == id);
        }

        public async Task<VerificationCode> GetLatestCodeAsync(
            string phoneNumber,
            VerificationPurpose purpose)
        {
            return await _context.VerificationCodes
                .Where(v => v.PhoneNumber == phoneNumber &&
                       v.Purpose == purpose &&
                       !v.IsUsed &&
                       v.ExpiryTime > DateTime.UtcNow)
                .OrderByDescending(v => v.CreatedAt)
                .FirstOrDefaultAsync();
        }

        public async Task UpdateAsync(VerificationCode code)
        {
            _context.Entry(code).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task<bool> IsRecentCodeExistsAsync(
            string phoneNumber,
            TimeSpan window)
        {
            var cutoffTime = DateTime.UtcNow.Subtract(window);
            return await _context.VerificationCodes
                .AnyAsync(v => v.PhoneNumber == phoneNumber &&
                              v.CreatedAt >= cutoffTime);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var code = await GetByIdAsync(id);
            if (code != null)
            {
                _context.VerificationCodes.Remove(code);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task DeleteAsync(VerificationCode code)
        {
            _context.VerificationCodes.Remove(code);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteExpiredCodesAsync()
        {
            var now = DateTime.UtcNow;
            var expiredCodes = _context.VerificationCodes
                .Where(v => v.ExpiryTime < now);

            _context.VerificationCodes.RemoveRange(expiredCodes);
            await _context.SaveChangesAsync();
        }
    }
}