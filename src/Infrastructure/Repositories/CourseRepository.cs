using Multilevelteam.Platform.Domain.Entities;
using Multilevelteam.Platform.Domain.Interfaces;
using Multilevelteam.Platform.Infrastructure.Data;

public class CourseRepository : ICourseRepository
{
    private readonly AppDbContext _context;

    public CourseRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task CreateCourseAsync(Course course)
    {
        await _context.Courses.AddAsync(course);
        await _context.SaveChangesAsync();
    }

}