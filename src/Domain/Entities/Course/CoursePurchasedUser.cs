using System;
namespace Multilevelteam.Platform.Domain.Entities
{
    public class CoursePurchasedUser
    {
        public Guid CourseId { get; set; }
        public Course Course { get; set; }

        public Guid UserId { get; set; }
        public User User { get; set; }

        public DateTime PurchasedAt { get; set; } = DateTime.UtcNow;
        public decimal PurchasePrice { get; set; }
    }
}