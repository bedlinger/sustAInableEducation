using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace sustAInableEducation_backend.Models
{
    public class ApplicationUser : IdentityUser
    {
        public ICollection<EnvironmentParticipant> Participations { get; set; }
    }
}
