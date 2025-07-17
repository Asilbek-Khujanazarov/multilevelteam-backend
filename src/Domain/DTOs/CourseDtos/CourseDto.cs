using System;
using System.Collections.Generic;
using Multilevelteam.Platform.Application.Dtos.lessonDtos;
using Multilevelteam.Platform.Domain.Entities;
using Multilevelteam.Platform.Domain.Enums;

namespace Multilevelteam.Platform.Application.Dtos.CourseDtos
{
    public class CourseDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public CourseType Type { get; set; }
        public decimal Price { get; set; }
        public Guid TeacherId { get; set; }
        public string CoverImageUrl { get; set; }
        public List<LessonDto> Lessons { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}