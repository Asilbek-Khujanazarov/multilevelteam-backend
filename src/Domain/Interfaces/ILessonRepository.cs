using Multilevelteam.Platform.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Multilevelteam.Platform.Application.Interfaces
{
    public interface ILessonRepository
    {
        Task<Lesson> GetByIdAsync(Guid id);
        Task<List<Lesson>> GetByCourseIdAsync(Guid courseId);
        Task<Lesson> AddAsync(Lesson lesson);
        Task UpdateAsync(Lesson lesson);
    }
}