using System;
using System.ComponentModel.DataAnnotations;

namespace Multilevelteam.Platform.Application.Dtos.CourseDtos
{
    public class CourseTeacherIdDto
    {
        [Required(ErrorMessage = "O'qituvchi ID si bo'sh bo'lmasligi kerak.")]
        public Guid TeacherId { get; set; }
    }
}