using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace sustAInableEducation_backend.Models
{
    public class Environment
    {
        public Guid Id { get; set; }
        [JsonIgnore]
        public Guid StoryId { get; set; }

        public ICollection<EnvironmentParticipant> Participants { get; set; } = new List<EnvironmentParticipant>();
        public Story Story { get; set; }
        [JsonIgnore]
        public EnvironmentAccessCode? AccessCode { get; set; }

        [MaxLength(256)]
        public string Title { get; set; }
        public uint VotingTimeSeconds { get; set; } = 30;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
