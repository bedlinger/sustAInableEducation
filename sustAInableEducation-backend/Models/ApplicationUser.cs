using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.Text.Json.Serialization;

namespace sustAInableEducation_backend.Models
{
    public class ApplicationUser : IdentityUser
    {
        public ICollection<EnvironmentParticipant> Participations { get; set; }

        #region overrides

        public override string Email { get; set; }

        [JsonIgnore]
        public override bool EmailConfirmed { get; set; }

        [JsonIgnore]
        public override bool TwoFactorEnabled { get; set; }

        [JsonIgnore]
        public override string PhoneNumber { get; set; }

        [JsonIgnore]
        public override bool PhoneNumberConfirmed { get; set; }

        [JsonIgnore]
        public override string PasswordHash { get; set; }

        [JsonIgnore]
        public override string SecurityStamp { get; set; }

        [JsonIgnore]
        public override bool LockoutEnabled { get; set; }

        [JsonIgnore]
        public override int AccessFailedCount { get; set; }

        #endregion
    }
}
