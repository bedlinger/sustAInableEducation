using System.ComponentModel.DataAnnotations;

namespace sustAInableEducation_backend.Models
{
    public class QuizQuestion
    {
        public Guid Id { get; set; }

        public ICollection<QuizChoice> Choices { get; set; } = new List<QuizChoice>();
        public ICollection<QuizResult> Results { get; set; } = new List<QuizResult>();

        public int Number { get; set; }
        public string Text { get; set; } = null!;
        public bool IsMultipleResponse { get; set; }
    }
}
