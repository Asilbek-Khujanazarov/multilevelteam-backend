using System;
using System.Collections.Generic;
using Multilevelteam.Platform.Application.Dtos.lessonDtos;
using Multilevelteam.Platform.Domain.Entities;
using Multilevelteam.Platform.Domain.Enums;

namespace Multilevelteam.Platform.Application.Dtos.CourseUpdateDtos
{
    public class CourseUpdateDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public CourseType Type { get; set; }
        public decimal Price { get; set; }
        public DateTime UpdatedAt { get; internal set; }
    }
}