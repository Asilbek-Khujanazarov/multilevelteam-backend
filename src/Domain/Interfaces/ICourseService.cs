
using Multilevelteam.Platform.Application.Dtos.CourseDtos;


namespace Multilevelteam.Platform.Application.Interfaces
{
    public interface ICourseService
    {
        Task<CourseDto> CreateAsync(CreateCourseDto dto, Guid teacherId);
        Task<List<CourseDto>> GetAllAsync();
        Task<CourseDto> GetByIdAsync(Guid id);
    }
}