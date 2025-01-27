using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using sustAInableEducation_backend.Models;

namespace sustAInableEducation_backend.Repository
{
    public class ApplicationUserValidator : IUserValidator<ApplicationUser>
    {
        private readonly ApplicationDbContext _context;

        public ApplicationUserValidator(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IdentityResult> ValidateAsync(UserManager<ApplicationUser> manager, ApplicationUser user)
        {
            var isRegistrationAllowed = await _context.Setting
                .Where(s => s.Id == "AllowRegistration")
                .Select(s => s.Value)
                .FirstOrDefaultAsync();

            if (isRegistrationAllowed != "true")
            {
                return IdentityResult.Failed(new IdentityError
                {
                    Code = "RegistrationNotAllowed",
                    Description = "Registration is currently not allowed."
                });
            }

            return IdentityResult.Success;
        }
    }
}
