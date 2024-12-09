using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace sustAInableEducation_backend.Models
{
    public class QuizChoice
    {
        [JsonIgnore]
        public Guid QuizQuestionId { get; set; }
        public int Number { get; set; }

        [MaxLength(1024)]
        public string Text { get; set; } = null!;
        [JsonIgnore]
        public bool IsCorrect { get; set; }
    }
}
