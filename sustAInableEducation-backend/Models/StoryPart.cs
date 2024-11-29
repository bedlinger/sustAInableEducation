using System.ComponentModel.DataAnnotations;

namespace sustAInableEducation_backend.Models
{
    public class StoryPart
    {
        public Guid Id { get; set; }

        public IEnumerable<StoryChoice> Choices { get; set; }

        public string Text { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
