using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace sustAInableEducation_backend.Models
{
    public class EnvironmentPIN
    {
        [JsonIgnore]
        public Guid EnvironmentId { get; set; }

        [JsonIgnore]
        public Environment Environment { get; set; }

        [MaxLength(8)]
        public string PIN { get; set; }
        public DateTime ExpiresAt { get; set; } = DateTime.Now.AddMinutes(1);
    }

    public class EnvironmentPINRequest
    {
        public string PIN { get; set; }
    }
}
