using System.ComponentModel.DataAnnotations;

namespace sustAInableEducation_backend.Models
{
    public class StoryPart
    {
        public Guid Id { get; set; }

        public Story Story { get; set; }
        public ICollection<StoryChoice> Choices { get; set; }

        [MaxLength(4096)]
        public string Text { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
