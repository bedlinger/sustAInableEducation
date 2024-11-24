using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace sustAInableEducation_backend.Models
{
    public class Story
    {
        public Guid Id { get; set; }
        public Guid? PresetId { get; set; }

        [JsonIgnore]
        public StoryPreset? Preset { get; set; }
        [JsonIgnore]
        public ICollection<StoryPart> Parts { get; set; } = new List<StoryPart>();

        [MaxLength(256)]
        public string Title { get; set; }
        public string? Prompt { get; set; }
        public int? Length { get; set; }
        public int? Creativity { get; set; }
    }
}
