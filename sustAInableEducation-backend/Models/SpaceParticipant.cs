using System.Text.Json.Serialization;

namespace sustAInableEducation_backend.Models;

public class SpaceParticipant
{
    public string UserId { get; set; } = null!;
    [JsonIgnore] public Guid SpaceId { get; set; }

    [JsonIgnore] public ApplicationUser User { get; set; } = null!;
    [JsonIgnore] public Space Space { get; set; } = null!;
    [JsonIgnore] public ICollection<Quiz> Quizzes { get; set; } = new List<Quiz>();

    public string UserName => User?.AnonUserName ?? "";
    public string? ProfileImage => User?.ProfileImage;
    public bool IsHost { get; set; }
    public bool IsOnline { get; set; }
    [JsonIgnore] public float? VoteImpact { get; set; }
    public float Impact { get; set; } = 0;
}