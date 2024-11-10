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
    }
}
