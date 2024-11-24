using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.Text.Json.Serialization;

namespace sustAInableEducation_backend.Models
{
    public class ApplicationUser : IdentityUser
    {
        [JsonIgnore]
        public ICollection<EnvironmentParticipant> Participations { get; set; }
    }

}
