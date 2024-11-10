using System.ComponentModel.DataAnnotations;

namespace sustAInableEducation_backend.Models
{
    public class StoryPreset
    {
        public Guid Id { get; set; }
        public Guid InitialPartId { get; set; }

        public StoryPresetPart InitialPart { get; set; }

        [MaxLength(256)]
        public string Title { get; set; }
    }
}
