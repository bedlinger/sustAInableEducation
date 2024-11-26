using System.ComponentModel.DataAnnotations;

namespace sustAInableEducation_backend.Models
{
    public class StoryPresetPart
    {
        public Guid Id { get; set; }
        public Guid PreviousId { get; set; }

        public ICollection<StoryPresetPart> NextParts { get; set; } = new List<StoryPresetPart>();

        public string Text { get; set; }
        public int ChoiceNumber { get; set; }
        [MaxLength(1024)]
        public string ChoiceText { get; set; }
    }
}
