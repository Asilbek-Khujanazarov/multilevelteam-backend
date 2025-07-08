using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Multilevelteam.Platform.Domain.Enums;

namespace Multilevelteam.Platform.Domain.Entities
{
    public class Course
    {
        public Course()
        {
            Lessons = new List<Lesson>();
            AllowedUsers = new List<CourseAllowedUser>();
            PurchasedUsers = new List<CoursePurchasedUser>();
        }

        [Key]
        public Guid Id { get; set; }

        [Required, MaxLength(100)]
        public string Title { get; set; }

        public string Description { get; set; }

        [Required]
        public CourseType Type { get; set; }

        public decimal Price { get; set; }

        [Required]
        public Guid TeacherId { get; set; }
        public virtual User Teacher { get; set; }

        // Private kurslar uchun allowed users
        public virtual ICollection<CourseAllowedUser> AllowedUsers { get; set; }

        // Public kurslar uchun purchased users
        public virtual ICollection<CoursePurchasedUser> PurchasedUsers { get; set; }

        // Kursga tegishli darslar
        public virtual ICollection<Lesson> Lessons { get; set; }

        public string? CoverImageUrl { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}