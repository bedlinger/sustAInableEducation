using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace sustAInableEducation_backend.Models
{
    public class StoryChoice
    {
        [JsonIgnore] public Guid StoryPartId { get; set; }
        public int Number { get; set; }
        [MaxLength(1024)] public string Text { get; set; } = null!;
        public uint NumberVotes { get; set; }
        [JsonIgnore] public float Impact { get; set; }
    }
}