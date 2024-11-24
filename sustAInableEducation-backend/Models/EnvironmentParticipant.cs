using System.Text.Json.Serialization;

namespace sustAInableEducation_backend.Models
{
    public class EnvironmentParticipant
    {
        public string UserId { get; set; }
        [JsonIgnore]
        public Guid EnvironmentId { get; set; }

        [JsonIgnore]
        public ApplicationUser User { get; set; }
        [JsonIgnore]
        public Environment Environment { get; set; }
        [JsonIgnore]
        public ICollection<Quiz> Quizzes { get; set; } = new List<Quiz>();

        public string UserName { get => User.UserName; }
        public bool IsHost { get; set; }
    }
}
