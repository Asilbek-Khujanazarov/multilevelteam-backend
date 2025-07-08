using System;
namespace Multilevelteam.Platform.Domain.Entities
{
    public class CourseAllowedUser
    {
        public Guid CourseId { get; set; }
        public Course Course { get; set; }

        public Guid UserId { get; set; }
        public User User { get; set; }

        public DateTime GrantedAt { get; set; } = DateTime.UtcNow;
    }
}