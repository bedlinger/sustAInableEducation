using System.ComponentModel.DataAnnotations;

namespace sustAInableEducation_backend.Models
{
    public class Quiz
    {
        public Guid Id { get; set; }

        public ICollection<QuizQuestion> Questions { get; set; } = new List<QuizQuestion>();

        [MaxLength(256)]
        public string Title { get; set; }
    }
}
