using AutoMapper;
using Multilevelteam.Platform.Domain.Entities;
using Multilevelteam.Platform.Domain.Interfaces;

public class CourseService : ICourseService
{
    private readonly ICourseRepository _courseRepository;
    private readonly IMapper _mapper;

    public CourseService(ICourseRepository courseRepository, IMapper mapper)
    {
        _mapper = mapper;
        _courseRepository = courseRepository;
    }
    
    public async Task<Course> CreateCourseAsync(CreateCourseDto courseDto)
    {
        var course = _mapper.Map<Course>(courseDto);
        await _courseRepository.CreateCourseAsync(course);
        return course;
    }
    
}