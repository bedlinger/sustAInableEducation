using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace sustAInableEducation_backend.Models
{
    public class Quiz
    {
        public Guid Id { get; set; }
        [JsonIgnore]
        public string UserId { get; set; } = null!;
        [JsonIgnore]
        public Guid EnvironmentId { get; set; }

        public ICollection<QuizQuestion> Questions { get; set; } = new List<QuizQuestion>();

        [MaxLength(256)]
        public string Title { get; set; } = null!;
        public uint NumberQuestions { get; set; }

        public IEnumerable<IGrouping<int, QuizResult>> Tries => Questions.Select(q => q.Results).SelectMany(r => r).GroupBy(r => r.TryNumber);
    }

    public enum QuizType
    {
        SingleResponse,
        MultipleResponse,
        TrueFalse
    }

    public class QuizRequest
    {
        public Guid EnvironmentId { get; set; }
        public ICollection<QuizType> Types { get; set; } = new List<QuizType>();
        public uint NumberQuestions { get; set; }
    }
}
