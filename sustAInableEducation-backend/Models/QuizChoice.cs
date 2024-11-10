using System.ComponentModel.DataAnnotations;

namespace sustAInableEducation_backend.Models
{
    public class QuizChoice
    {
        public Guid QuizQuestionId { get; set; }
        public int Number { get; set; }

        [MaxLength(1024)]
        public string Text { get; set; }
        public bool IsCorrect { get; set; }
    }
}
