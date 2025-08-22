using AutoMapper;
using Multilevelteam.Platform.Domain.Entities;
using Multilevelteam.Platform.Application.Dtos;
using Multilevelteam.Platform.Domain.Enums;
using Multilevelteam.Platform.Application.Dtos.CourseDtos;
using Multilevelteam.Platform.Application.Dtos.lessonDtos;
using Multilevelteam.Platform.Application.Dtos.CourseUpdateDtos;

namespace Multilevelteam.Platform.Application.Mapping
{
    public class CourseProfile : Profile
    {
        public CourseProfile()
        {
            CreateMap<Course, CourseDto>()
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type.ToString()))
                .ForMember(dest => dest.Lessons, opt => opt.MapFrom(src => src.Lessons));
            CreateMap<CreateCourseDto, Course>();

            CreateMap<Lesson, LessonDto>();
            CreateMap<CreateLessonDto, Lesson>();
            CreateMap<Course, CourseTeacherIdDto>().ReverseMap();
            CreateMap<Course, CourseUpdateDto>().ReverseMap();
        }
    }
}