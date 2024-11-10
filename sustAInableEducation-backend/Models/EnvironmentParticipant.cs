namespace sustAInableEducation_backend.Models
{
    public class EnvironmentParticipant
    {
        public string UserId { get; set; }
        public Guid EnvironmentId { get; set; }

        public ApplicationUser User { get; set; }
        public Environment Environment { get; set; }
        public ICollection<Quiz> Quizzes { get; set; } = new List<Quiz>();
    }
}
