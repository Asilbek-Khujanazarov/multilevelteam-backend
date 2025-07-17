using Multilevelteam.Platform.Application.Interfaces;
using Multilevelteam.Platform.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Multilevelteam.Platform.Infrastructure.Data
{
    public class LessonRepository : ILessonRepository
    {
        private readonly AppDbContext _context;
        public LessonRepository(AppDbContext context) => _context = context;

        public async Task<Lesson> GetByIdAsync(Guid id) =>
            await _context.Lessons.FirstOrDefaultAsync(l => l.Id == id);

        public async Task<List<Lesson>> GetByCourseIdAsync(Guid courseId) =>
            await _context.Lessons.Where(l => l.CourseId == courseId).ToListAsync();

        public async Task<Lesson> AddAsync(Lesson lesson)
        {
            _context.Lessons.Add(lesson);
            await _context.SaveChangesAsync();
            return lesson;
        }

        public async Task UpdateAsync(Lesson lesson)
        {
            _context.Lessons.Update(lesson);
            await _context.SaveChangesAsync();
        }
    }
}