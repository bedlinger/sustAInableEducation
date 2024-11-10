using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.EntityFrameworkCore;
using sustAInableEducation_backend.Repository;

namespace sustAInableEducation_backend.Models
{
    public class Environment
    {
        public Guid Id { get; set; }

        public ICollection<EnvironmentParticipant> Participants { get; set; } = new List<EnvironmentParticipant>();
        public Story? Story { get; set; }

        [MaxLength(256)]
        public string Title { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
