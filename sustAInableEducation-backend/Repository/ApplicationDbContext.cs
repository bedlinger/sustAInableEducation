using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using sustAInableEducation_backend.Models;
using System.Text.Json;
using Space = sustAInableEducation_backend.Models.Space;

namespace sustAInableEducation_backend.Repository
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<SpaceParticipant>()
                .HasKey(e => new { e.SpaceId, e.UserId });
            builder.Entity<SpaceAccessCode>()
                .HasKey(e => e.Code);

            builder.Entity<Story>()
                .OwnsOne(e => e.Result);
            builder.Entity<StoryChoice>()
                .HasKey(e => new { e.StoryPartId, e.Number });

            builder.Entity<QuizChoice>()
                .HasKey(e => new { e.QuizQuestionId, e.Number });
            builder.Entity<QuizResult>()
                .HasKey(e => new { e.QuizQuestionId, e.TryNumber });
        }

        public DbSet<sustAInableEducation_backend.Models.Space> Space { get; set; } = default!;
        public DbSet<sustAInableEducation_backend.Models.SpaceParticipant> SpaceParticipant { get; set; } = default!;
        public DbSet<sustAInableEducation_backend.Models.SpaceAccessCode> SpaceAccessCode { get; set; } = default!;
        public DbSet<sustAInableEducation_backend.Models.Quiz> Quiz { get; set; } = default!;
        public DbSet<sustAInableEducation_backend.Models.QuizChoice> QuizChoice { get; set; } = default!;
        public DbSet<sustAInableEducation_backend.Models.QuizQuestion> QuizQuestion { get; set; } = default!;
        public DbSet<sustAInableEducation_backend.Models.QuizResult> QuizResult { get; set; } = default!;
        public DbSet<sustAInableEducation_backend.Models.Story> Story { get; set; } = default!;
        public DbSet<sustAInableEducation_backend.Models.StoryChoice> StoryChoice { get; set; } = default!;
        public DbSet<sustAInableEducation_backend.Models.StoryPart> StoryPart { get; set; } = default!;

        public IQueryable<Space> SpaceWithStory => Space
                .Include(e => e.Story)
                .ThenInclude(s => s.Parts.OrderBy(p => p.CreatedAt))
                .ThenInclude(p => p.Choices.OrderBy(p => p.Number));
        public IQueryable<Space> SpaceWithAll => SpaceWithStory
                .Include(e => e.Participants)
                .ThenInclude(e => e.User);

        public IQueryable<Quiz> QuizWithAll => Quiz
                .Include(q => q.Questions)
                .ThenInclude(q => q.Choices)
                .Include(q => q.Questions)
                .ThenInclude(q => q.Results);

        public async Task<bool> IsParticipant(string userId, Guid spaceId)
        {
            return await SpaceParticipant.AnyAsync(p => p.UserId == userId && p.SpaceId == spaceId);
        }
        
        public async Task<bool> IsHost(string userId, Guid spaceId)
        {
            return await SpaceParticipant.AnyAsync(p => p.UserId == userId && p.SpaceId == spaceId && p.IsHost);
        }
    }
}
