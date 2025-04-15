using System.Text.Json.Serialization;

namespace sustAInableEducation_backend.Models;

public class QuizResult
{
    public Guid QuizQuestionId { get; set; }
    [JsonIgnore] public int TryNumber { get; set; }

    public bool IsCorrect { get; set; }
}