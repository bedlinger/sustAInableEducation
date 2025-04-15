using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using sustAInableEducation_backend.Models.Validation;

namespace sustAInableEducation_backend.Models;

public class Story
{
    [JsonIgnore] public Guid Id { get; set; }

    public ICollection<StoryPart> Parts { get; set; } = new List<StoryPart>();
    public StoryResult? Result { get; set; }

    [MaxLength(256)] public string? Title { get; set; }
    public string Topic { get; set; } = null!;
    public uint Length { get; set; }
    public float Temperature { get; set; } = 0.8f;
    public float TopP { get; set; } = 0.7f;
    public float TotalImpact { get; set; } = 0;
    public TargetGroup TargetGroup { get; set; }

    [JsonIgnore] public bool IsComplete => Parts.Count >= Length;
}

public enum TargetGroup
{
    PrimarySchool,
    MiddleSchool,
    HighSchool
}

public class StoryResult
{
    public string Text { get; set; } = null!;
    public string Summary { get; set; } = null!;
    public string[] PositiveChoices { get; set; } = [];
    public string[] NegativeChoices { get; set; } = [];
    public string[] Learnings { get; set; } = [];
    public string[] DiscussionQuestions { get; set; } = [];
    public string? Image { get; set; }
}

public class StoryRequest
{
    public string Topic { get; set; } = null!;

    [Range(3, 10)] public uint Length { get; set; } = 3;

    [Range(0, 2)] public float Temperature { get; set; } = 0.8f;

    [Range(0, 1, MinimumIsExclusive = true)]
    public float TopP { get; set; } = 0.7f;

    [ValidEnum] public TargetGroup TargetGroup { get; set; }
}