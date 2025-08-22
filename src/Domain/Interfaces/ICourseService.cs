
using Multilevelteam.Platform.Application.Dtos.CourseDtos;
using Multilevelteam.Platform.Application.Dtos.CourseUpdateDtos;


namespace Multilevelteam.Platform.Application.Interfaces
{
    public interface ICourseService
    {
        Task<CourseDto> CreateAsync(CreateCourseDto dto, Guid teacherId);
        Task<List<CourseDto>> GetAllAsync();
        Task<CourseDto> GetByIdAsync(Guid id);
        Task<CourseDto> UpdateAsync(CourseUpdateDto dto);
        Task<bool> UpdateTeacherAsync(Guid courseId, CourseTeacherIdDto dto);
        Task<bool> DeleteAsync(Guid id);
    }
}