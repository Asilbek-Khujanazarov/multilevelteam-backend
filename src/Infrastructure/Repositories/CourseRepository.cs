using Multilevelteam.Platform.Application.Interfaces;
using Multilevelteam.Platform.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Multilevelteam.Platform.Infrastructure.Data
{
    public class CourseRepository : ICourseRepository
    {
        private readonly AppDbContext _context;
        public CourseRepository(AppDbContext context) => _context = context;

        public async Task<Course> GetByIdAsync(Guid id) =>
            await _context.Courses.Include(c => c.Lessons).FirstOrDefaultAsync(c => c.Id == id);

        public async Task<List<Course>> GetAllAsync() =>
            await _context.Courses.Include(c => c.Lessons).ToListAsync();

        public async Task<Course> AddAsync(Course course)
        {
            _context.Courses.Add(course);
            await _context.SaveChangesAsync();
            return course;
        }

        public async Task UpdateAsync(Course course)
        {
            _context.Courses.Update(course);
            await _context.SaveChangesAsync();
        }
    }
}