// src/Infrastructure/Data/AppDbContext.cs
using Microsoft.EntityFrameworkCore;
using Multilevelteam.Platform.Domain.Entities;

namespace Multilevelteam.Platform.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<TelegramUser> TelegramUsers { get; set; }
        public DbSet<VerificationCode> VerificationCodes { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Answer> Answers { get; set; }
        public DbSet<TestSession> TestSessions { get; set; }
        public DbSet<TestSessionQuestion> TestSessionQuestions { get; set; }
         public DbSet<Course> Courses { get; set; }
        public DbSet<CourseAllowedUser> CourseAllowedUsers { get; set; }
        public DbSet<CoursePurchasedUser> CoursePurchasedUsers { get; set; }
        public DbSet<Lesson> Lessons { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
             // User-Course (Teacher) 1-ko‘p
            modelBuilder.Entity<Course>()
                .HasOne(c => c.Teacher)
                .WithMany(u => u.CreatedCourses)
                .HasForeignKey(c => c.TeacherId)
                .OnDelete(DeleteBehavior.Restrict);

            // CourseAllowedUser (Private)
            modelBuilder.Entity<CourseAllowedUser>()
                .HasKey(x => new { x.CourseId, x.UserId });

            modelBuilder.Entity<CourseAllowedUser>()
                .HasOne(x => x.Course)
                .WithMany(c => c.AllowedUsers)
                .HasForeignKey(x => x.CourseId);

            modelBuilder.Entity<CourseAllowedUser>()
                .HasOne(x => x.User)
                .WithMany(u => u.AllowedCourses)
                .HasForeignKey(x => x.UserId);

            // CoursePurchasedUser (Public)
            modelBuilder.Entity<CoursePurchasedUser>()
                .HasKey(x => new { x.CourseId, x.UserId });

            modelBuilder.Entity<CoursePurchasedUser>()
                .HasOne(x => x.Course)
                .WithMany(c => c.PurchasedUsers)
                .HasForeignKey(x => x.CourseId);

            modelBuilder.Entity<CoursePurchasedUser>()
                .HasOne(x => x.User)
                .WithMany(u => u.PurchasedCourses)
                .HasForeignKey(x => x.UserId);

            // Lesson - Course 1-ko‘p
            modelBuilder.Entity<Lesson>()
                .HasOne(l => l.Course)
                .WithMany(c => c.Lessons)
                .HasForeignKey(l => l.CourseId);

            // User konfiguratsiyasi
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.PhoneNumber).IsRequired().HasMaxLength(15);
                entity.Property(e => e.PasswordHash).IsRequired();
                entity.Property(e => e.FirstName).IsRequired().HasMaxLength(50);
                entity.Property(e => e.LastName).IsRequired().HasMaxLength(50);
                entity.Property(e => e.IsVerified).HasDefaultValue(false);
                entity.Property(e => e.RefreshToken).IsRequired();
                entity.HasIndex(e => e.PhoneNumber).IsUnique();

                // TelegramUser bilan bog'lanish
                entity.HasOne(u => u.TelegramUser)
                    .WithOne(t => t.User)
                    .HasForeignKey<TelegramUser>(t => t.UserId)
                    .OnDelete(DeleteBehavior.SetNull);
            });

            // TelegramUser konfiguratsiyasi
            modelBuilder.Entity<TelegramUser>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.PhoneNumber).IsRequired().HasMaxLength(15);
                entity.Property(e => e.ChatId).IsRequired();
                entity.Property(e => e.LastInteractionAt).IsRequired();
                entity.HasIndex(e => e.PhoneNumber).IsUnique();
                entity.HasIndex(e => e.ChatId).IsUnique();
            });

            // VerificationCode konfiguratsiyasi
            modelBuilder.Entity<VerificationCode>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.PhoneNumber).IsRequired().HasMaxLength(15);
                entity.Property(e => e.Code).IsRequired().HasMaxLength(6);
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
                entity.HasIndex(e => new { e.PhoneNumber, e.Purpose });

            });
            modelBuilder.Entity<Question>()
            .HasMany(q => q.Answers)
            .WithOne(a => a.Question)
            .HasForeignKey(a => a.QuestionId)
            .OnDelete(DeleteBehavior.Cascade);
        }
    }
}


