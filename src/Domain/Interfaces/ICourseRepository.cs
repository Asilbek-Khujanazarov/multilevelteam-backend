using Multilevelteam.Platform.Application.Dtos.CourseDtos;
using Multilevelteam.Platform.Application.Dtos.CourseUpdateDtos;
using Multilevelteam.Platform.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Multilevelteam.Platform.Application.Interfaces
{
    public interface ICourseRepository
    {
        Task<Course> GetByIdAsync(Guid id);
        Task<List<Course>> GetAllAsync();
        Task<Course> AddAsync(Course course);
        Task UpdateAsync(Course course);
        Task<bool> UpdateTeacherAsync(Guid courseId, Guid teacherId);
        Task<bool> DeleteAsync(Guid id);


    }
}