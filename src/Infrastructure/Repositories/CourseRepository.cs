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
        public async Task<bool> UpdateTeacherAsync(Guid courseId, Guid teacherId)
        {
            var course = await _context.Courses.FirstOrDefaultAsync(c => c.Id == courseId);
            if (course == null) return false;

            // ✅ Teacher mavjudligini tekshirish
            var teacher = await _context.Users.FirstOrDefaultAsync(u => u.Id == teacherId);
            if (teacher == null)
                throw new Exception("Bunday Teacher mavjud emas!");

            course.TeacherId = teacherId;
            course.UpdatedAt = DateTime.UtcNow;

            _context.Courses.Update(course);
            await _context.SaveChangesAsync();
            return true;
        }


        public async Task<bool> DeleteAsync(Guid id)
        {
            var course = await _context.Courses.FirstOrDefaultAsync(c => c.Id == id);
            if (course == null) return false;

            _context.Courses.Remove(course);
            await _context.SaveChangesAsync();
            return true;
        }



    }
}