using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace sustAInableEducation_backend.Models
{
    public class SpaceAccessCode
    {
        [JsonIgnore] public Guid SpaceId { get; set; }

        [JsonIgnore] public Space Space { get; set; } = null!;

        [MaxLength(8)] public string Code { get; set; } = null!;
        public DateTime ExpiresAt { get; set; } = DateTime.Now.AddMinutes(10);
    }

    public class SpaceAccessCodeRequest
    {
        public string Code { get; set; } = null!;
    }
}