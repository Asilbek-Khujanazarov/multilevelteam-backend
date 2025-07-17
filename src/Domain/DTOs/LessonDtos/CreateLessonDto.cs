using System;

namespace Multilevelteam.Platform.Application.Dtos.lessonDtos
{
    public class CreateLessonDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public int Order { get; set; }
        public string Content { get; set; }
        public string FilePublicId { get; set; }
        public bool IsPreview { get; set; }
        public Guid CourseId { get; set; }
    }
}