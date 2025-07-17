using Multilevelteam.Platform.Application.Dtos.lessonDtos;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Multilevelteam.Platform.Application.Interfaces
{
    public interface ILessonService
    {
        Task<LessonDto> CreateAsync(CreateLessonDto dto);
        Task<List<LessonDto>> GetByCourseIdAsync(Guid courseId);
    }
}