using AptitudeTestApp.Data;
using AptitudeTestApp.Data.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AptitudeTestApp.Infrastructure.Persistence
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options)
    {
        public DbSet<University> Universities { get; set; }
        public DbSet<QuestionCategory> QuestionCategories { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<QuestionOption> QuestionOptions { get; set; }
        public DbSet<TestSession> TestSessions { get; set; }
        public DbSet<TestSessionQuestion> TestSessionQuestions { get; set; }
        public DbSet<StudentSubmission> StudentSubmissions { get; set; }
        public DbSet<StudentAnswer> StudentAnswers { get; set; }
        public DbSet<AntiCheatLog> AntiCheatLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            foreach (var entityType in builder.Model.GetEntityTypes())
            {
                var clrType = entityType.ClrType;

                // Check if it inherits from BaseEntity<Guid>
                if (clrType.BaseType != null &&
                    clrType.BaseType.IsGenericType &&
                    clrType.BaseType.GetGenericTypeDefinition() == typeof(BaseEntity<>))
                {
                    var property = clrType.GetProperty("CreatorId");
                    if (property != null)
                    {
                        builder.Entity(clrType).HasIndex("CreatorId");
                    }
                }
            }

            // University
            builder.Entity<University>(entity =>
            {
                entity.HasIndex(e => e.Code).IsUnique();
                entity.Property(e => e.Id).HasDefaultValueSql("NEWID()");
            });

            // QuestionCategory
            builder.Entity<QuestionCategory>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("NEWID()");
            });

            // Question
            builder.Entity<Question>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("NEWID()");
                entity.HasOne(d => d.Category)
                    .WithMany(p => p.Questions)
                    .HasForeignKey(d => d.CategoryId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // QuestionOption
            builder.Entity<QuestionOption>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("NEWID()");
                entity.HasOne(d => d.Question)
                    .WithMany(p => p.Options)
                    .HasForeignKey(d => d.QuestionId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // TestSession
            builder.Entity<TestSession>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("NEWID()");
                entity.HasIndex(e => e.Token).IsUnique();
                entity.HasOne(d => d.University)
                    .WithMany(p => p.TestSessions)
                    .HasForeignKey(d => d.UniversityId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // TestSessionQuestion
            builder.Entity<TestSessionQuestion>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("NEWID()");
                entity.HasIndex(e => new { e.TestSessionId, e.QuestionId }).IsUnique();
                entity.HasOne(d => d.TestSession)
                    .WithMany(p => p.TestSessionQuestions)
                    .HasForeignKey(d => d.TestSessionId)
                    .OnDelete(DeleteBehavior.Cascade);
                entity.HasOne(d => d.Question)
                    .WithMany(p => p.TestSessionQuestions)
                    .HasForeignKey(d => d.QuestionId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // StudentSubmission
            builder.Entity<StudentSubmission>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("NEWID()");
                entity.HasOne(d => d.TestSession)
                    .WithMany(p => p.StudentSubmissions)
                    .HasForeignKey(d => d.TestSessionId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // StudentAnswer
            builder.Entity<StudentAnswer>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("NEWID()");
                entity.HasIndex(e => new { e.SubmissionId, e.QuestionId }).IsUnique();
                entity.HasOne(d => d.Submission)
                    .WithMany(p => p.StudentAnswers)
                    .HasForeignKey(d => d.SubmissionId)
                    .OnDelete(DeleteBehavior.Cascade);
                entity.HasOne(d => d.Question)
                    .WithMany(p => p.StudentAnswers)
                    .HasForeignKey(d => d.QuestionId)
                    .OnDelete(DeleteBehavior.Cascade);
                entity.HasOne(d => d.SelectedOption)
                    .WithMany(p => p.StudentAnswers)
                    .HasForeignKey(d => d.SelectedOptionId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // AntiCheatLog
            builder.Entity<AntiCheatLog>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("NEWID()");
                entity.HasOne(d => d.Submission)
                    .WithMany(p => p.AntiCheatLogs)
                    .HasForeignKey(d => d.SubmissionId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}
