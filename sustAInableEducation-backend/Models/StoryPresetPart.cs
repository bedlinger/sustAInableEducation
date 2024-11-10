namespace sustAInableEducation_backend.Models
{
    public class StoryPresetPart
    {
        public Guid Id { get; set; }
        public Guid PreviousId { get; set; }

        public ICollection<StoryPresetPart> NextParts { get; set; }

        public string Text { get; set; }
        public int ChoiceNumber { get; set; }
        public string ChoiceText { get; set; }
    }
}
