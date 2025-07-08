using System;
using System.ComponentModel.DataAnnotations;

namespace Multilevelteam.Platform.Domain.Entities
{
    public class Lesson
    {
        [Key]
        public Guid Id { get; set; }

        [Required, MaxLength(100)]
        public string Title { get; set; }

        public string Description { get; set; }
        public int Order { get; set; }
        public string Content { get; set; }
        public string? FilePublicId { get; set; }
        [Required]
        public Guid CourseId { get; set; }
        public virtual Course Course { get; set; }
        public bool IsPreview { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}