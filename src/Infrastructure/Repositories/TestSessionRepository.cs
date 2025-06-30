
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autotest.Platform.Domain.Entities;
using Autotest.Platform.Domain.Interfaces;
using Autotest.Platform.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Autotest.Platform.Infrastructure.Data;

public class TestSessionRepository : ITestSessionRepository
{
    private readonly AppDbContext _context;

    public TestSessionRepository(AppDbContext context) => _context = context;

    public async Task<TestSession> CreateAsync(TestSession session)
    {
        _context.TestSessions.Add(session);
        await _context.SaveChangesAsync();
        return session;
    }

    public async Task<TestSession> GetByIdAsync(Guid id)
    {
        return await _context.TestSessions
            .Include(x => x.Questions)
                .ThenInclude(q => q.Question)
                    .ThenInclude(q => q.Answers)
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task UpdateAsync(TestSession session)
    {
        _context.TestSessions.Update(session);
        await _context.SaveChangesAsync();
    }

    public async Task<List<TestSession>> GetLastSessionsAsync(Guid userId, int count)
    {
        return await _context.TestSessions
            .Where(x => x.UserId == userId)
            .OrderByDescending(x => x.StartedAt)
            .Take(count)
            .Include(x => x.Questions)
            .ToListAsync();
    }

    public async Task<List<TestSession>> GetExpiredSessionsAsync()
    {
        return await _context.TestSessions
            .Where(x => !x.IsFinished && x.StartedAt.AddMinutes(x.DurationMinutes) < DateTime.UtcNow)
            .Include(x => x.Questions)
                .ThenInclude(q => q.Question)
                    .ThenInclude(q => q.Answers)
            .ToListAsync();
    }
}
