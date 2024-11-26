using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace sustAInableEducation_backend.Models
{
    public class EnvironmentAccessCode
    {
        [JsonIgnore]
        public Guid EnvironmentId { get; set; }

        [JsonIgnore]
        public Environment Environment { get; set; }

        [MaxLength(8)]
        public string Code { get; set; }
        public DateTime ExpiresAt { get; set; } = DateTime.Now.AddMinutes(1);
    }

    public class EnvironmentAccessCodeRequest
    {
        public string Code { get; set; }
    }
}
