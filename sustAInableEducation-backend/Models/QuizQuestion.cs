using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace sustAInableEducation_backend.Models
{
    public class QuizQuestion
    {
        public Guid Id { get; set; }

        public ICollection<QuizChoice> Choices { get; set; } = new List<QuizChoice>();
        [JsonIgnore]
        public ICollection<QuizResult> Results { get; set; } = new List<QuizResult>();

        public int Number { get; set; }
        public string Text { get; set; } = null!;
        public bool IsMultipleResponse { get; set; }
    }

    public class QuizQuestionResponse
    {
        public Guid QuestionId { get; set; }
        public ICollection<int> Response { get; set; } = new List<int>();
    }
}
