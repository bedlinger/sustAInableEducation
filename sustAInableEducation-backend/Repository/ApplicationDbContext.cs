using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using sustAInableEducation_backend.Models;
using Environment = sustAInableEducation_backend.Models.Environment;

namespace sustAInableEducation_backend.Repository
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<EnvironmentParticipant>()
                .HasKey(e => new { e.EnvironmentId, e.UserId });
            builder.Entity<StoryChoice>()
                .HasKey(e => new { e.StoryPartId, e.Number });
            builder.Entity<QuizChoice>()
                .HasKey(e => new { e.QuizQuestionId, e.Number });
            builder.Entity<QuizResult>()
                .HasKey(e => new { e.QuizQuestionId, e.TryNumber });
        }

        public DbSet<sustAInableEducation_backend.Models.Environment> Environment { get; set; } = default!;
        public DbSet<sustAInableEducation_backend.Models.EnvironmentParticipant> EnvironmentParticipant { get; set; } = default!;
        public DbSet<sustAInableEducation_backend.Models.Quiz> Quiz { get; set; } = default!;
        public DbSet<sustAInableEducation_backend.Models.QuizChoice> QuizChoice { get; set; } = default!;
        public DbSet<sustAInableEducation_backend.Models.QuizQuestion> QuizQuestion { get; set; } = default!;
        public DbSet<sustAInableEducation_backend.Models.QuizResult> QuizResult { get; set; } = default!;
        public DbSet<sustAInableEducation_backend.Models.Story> Story { get; set; } = default!;
        public DbSet<sustAInableEducation_backend.Models.StoryChoice> StoryChoice { get; set; } = default!;
        public DbSet<sustAInableEducation_backend.Models.StoryPart> StoryPart { get; set; } = default!;
        public DbSet<sustAInableEducation_backend.Models.StoryPreset> StoryPreset { get; set; } = default!;
        public DbSet<sustAInableEducation_backend.Models.StoryPresetPart> StoryPresetPart { get; set; } = default!;

        public async Task<bool> IsParticipant(string userId, Guid environmentId)
        {
            return await EnvironmentParticipant.AnyAsync(p => p.UserId == userId && p.EnvironmentId == environmentId);
        }
        
        public async Task<bool> IsHost(string userId, Guid environmentId)
        {
            return await EnvironmentParticipant.AnyAsync(p => p.UserId == userId && p.EnvironmentId == environmentId && p.IsHost);
        }
    }
}
