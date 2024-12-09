using System.ComponentModel.DataAnnotations;

namespace sustAInableEducation_backend.Models
{
    public class Quiz
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid EnvironmentId { get; set; }

        public ICollection<QuizQuestion> Questions { get; set; } = [];

        [MaxLength(256)]
        public string Title { get; set; } = null!;
    }
}
