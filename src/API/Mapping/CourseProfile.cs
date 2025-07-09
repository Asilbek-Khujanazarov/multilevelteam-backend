using AutoMapper;
using Multilevelteam.Platform.Domain.Entities;

public class CourseProfile : Profile
{
    CourseProfile()
    {
        CreateMap<Course, CreateCourseDto>();
    }
}