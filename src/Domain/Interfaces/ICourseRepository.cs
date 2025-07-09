using Multilevelteam.Platform.Domain.Entities;

namespace Multilevelteam.Platform.Domain.Interfaces
{
    public interface ICourseRepository
    {
        Task CreateCourseAsync (Course course);
    }
}