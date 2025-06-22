
// src/Domain/Interfaces/IVerificationCodeRepository.cs
using Autotest.Platform.Domain.Entities;
using Autotest.Platform.Domain.Enums;

namespace Autotest.Platform.Domain.Interfaces
{
    public interface IVerificationCodeRepository
    {
        Task<VerificationCode> CreateAsync(VerificationCode code);
        Task<VerificationCode> GetByIdAsync(Guid id);
        Task<VerificationCode> GetLatestCodeAsync(string phoneNumber, VerificationPurpose purpose);
        Task UpdateAsync(VerificationCode code);
        Task<bool> IsRecentCodeExistsAsync(string phoneNumber, TimeSpan window);
        Task<bool> DeleteAsync(Guid id);
        Task DeleteAsync(VerificationCode code);
        Task DeleteExpiredCodesAsync();
    }
}