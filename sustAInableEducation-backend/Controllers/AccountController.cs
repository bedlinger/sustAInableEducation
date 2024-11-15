using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using sustAInableEducation_backend.Models;
using sustAInableEducation_backend.Repository;
using System.Security.Claims;

namespace sustAInableEducation_backend.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class AccountController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private string userId { get; set; }
        private ApplicationUser user { get; set; }

        public AccountController(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            userId = httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            user = _context.Users.Find(userId)!;
        }

        // GET: api/account
        [HttpGet]
        public ActionResult<ApplicationUser> GetAccount()
        {
            return user;
        }
    }
}
