// src/Infrastructure/Data/AppDbContext.cs
using Microsoft.EntityFrameworkCore;
using Autotest.Platform.Domain.Entities;

namespace Autotest.Platform.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<VerificationCode> VerificationCodes { get; set; }
        public DbSet<TelegramUser> TelegramUsers { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // User konfiguratsiyasi
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.PhoneNumber)
                    .IsRequired()
                    .HasMaxLength(15);

                entity.HasIndex(e => e.PhoneNumber)
                    .IsUnique();

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.PasswordHash)
                    .IsRequired();

                entity.Property(e => e.RegisteredDate)
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                // VerificationCode bilan relationship
                entity.HasMany(e => e.VerificationCodes)
                    .WithOne(e => e.User)
                    .HasForeignKey(e => e.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // VerificationCode konfiguratsiyasi
            modelBuilder.Entity<VerificationCode>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.PhoneNumber)
                    .IsRequired()
                    .HasMaxLength(15);

                entity.Property(e => e.Code)
                    .IsRequired()
                    .HasMaxLength(6);

                entity.Property(e => e.CreatedAt)
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.ExpiryTime)
                    .IsRequired();
            });

            // TelegramUser konfiguratsiyasi
            modelBuilder.Entity<TelegramUser>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.ChatId)
                    .IsRequired();

                entity.Property(e => e.PhoneNumber)
                    .IsRequired()
                    .HasMaxLength(15);

                entity.HasIndex(e => e.PhoneNumber)
                    .IsUnique();

                entity.HasIndex(e => e.ChatId)
                    .IsUnique();

                entity.Property(e => e.CreatedAt)
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                // User bilan relationship
                entity.HasOne(e => e.User)
                    .WithOne()
                    .HasForeignKey<TelegramUser>(e => e.UserId)
                    .OnDelete(DeleteBehavior.SetNull);
            });
        }
    }
}


