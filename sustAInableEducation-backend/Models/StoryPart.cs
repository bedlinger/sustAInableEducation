using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace sustAInableEducation_backend.Models
{
    public class StoryPart
    {
        [JsonIgnore]
        public Guid Id { get; set; }

        public IEnumerable<StoryChoice> Choices { get; set; } = new List<StoryChoice>();

        public string Text { get; set; } = null!;
        [JsonIgnore]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime? VotingEndAt { get; set; }

        public uint? ChosenNumber { get; set; }
        [JsonIgnore]
        public bool IsVotingActive => VotingEndAt.HasValue && VotingEndAt.Value > DateTime.Now;
    }
}
