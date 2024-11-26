namespace sustAInableEducation_backend.Models
{
    public class QuizResult
    {
        public Guid QuizQuestionId { get; set; }
        public int TryNumber { get; set; }

        public bool IsCorrect { get; set; }
    }
}
