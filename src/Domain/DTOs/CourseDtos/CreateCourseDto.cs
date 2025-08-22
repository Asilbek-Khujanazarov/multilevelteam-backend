using System;
using System.Collections.Generic;
using Multilevelteam.Platform.Domain.Enums;

namespace Multilevelteam.Platform.Application.Dtos.CourseDtos
{
     public class CreateCourseDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public CourseType Type { get; set; }
        public decimal Price { get; set; }
        // public string CoverImageUrl { get; set; }
    }

}