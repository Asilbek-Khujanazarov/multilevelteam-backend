using Autotest.Platform.Domain.Entities;

public interface ITestSessionRepository
{
    Task<TestSession> CreateAsync(TestSession session);
    Task<TestSession> GetByIdAsync(Guid id);
    Task UpdateAsync(TestSession session);
    Task<List<TestSession>> GetLastSessionsAsync(Guid userId, int count);
}