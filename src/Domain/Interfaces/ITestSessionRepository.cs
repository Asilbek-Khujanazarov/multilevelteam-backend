using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Multilevelteam.Platform.Domain.Entities;

namespace Multilevelteam.Platform.Domain.Interfaces;

public interface ITestSessionRepository
{
    Task<TestSession> CreateAsync(TestSession session);
    Task<TestSession> GetByIdAsync(Guid id);
    Task UpdateAsync(TestSession session);
    Task<List<TestSession>> GetLastSessionsAsync(Guid userId, int count);
    Task<List<TestSession>> GetExpiredSessionsAsync();
}
