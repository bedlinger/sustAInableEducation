using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using sustAInableEducation_backend.Repository;
using System.Text.Json.Serialization;

namespace sustAInableEducation_backend.Models
{
    public class ApplicationUser : IdentityUser
    {
        [JsonIgnore]
        public ICollection<EnvironmentParticipant> Participations { get; set; }

        public string AnonUserName { get; set; } = UserNameGenService.GenerateUserName();
    }

}
