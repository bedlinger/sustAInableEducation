using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using sustAInableEducation_backend.Models;
using sustAInableEducation_backend.Repository;
using Environment = sustAInableEducation_backend.Models.Environment;

namespace sustAInableEducation_backend.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class EnvironmentsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private string userId { get; set; }
        private ApplicationUser user { get; set; }

        public EnvironmentsController(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            userId = httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            user = _context.Users.Find(userId)!;
        }

        // GET: api/Environments
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Environment>>> GetEnvironment()
        {
            return await _context.Environment.ToListAsync();
        }

        // GET: api/Environments/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Environment>> GetEnvironment(Guid id)
        {
            var environment = await _context.Environment.FindAsync(id);

            if (environment == null)
            {
                return NotFound();
            }

            return environment;
        }

        // PUT: api/Environments/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEnvironment(Guid id, Environment environment)
        {
            if (id != environment.Id)
            {
                return BadRequest();
            }

            _context.Entry(environment).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EnvironmentExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Environments
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Environment>> PostEnvironment(Environment environment)
        {
            environment.Participants = new List<EnvironmentParticipant>()
            {
                new EnvironmentParticipant()
                {
                    UserId = userId,
                    User = user,
                    EnvironmentId = environment.Id,
                    IsHost = true
                }
            };
            _context.Environment.Add(environment);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetEnvironment", new { id = environment.Id }, environment);
        }

        // DELETE: api/Environments/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEnvironment(Guid id)
        {
            var environment = await _context.Environment.FindAsync(id);
            if (environment == null)
            {
                return NotFound();
            }

            _context.Environment.Remove(environment);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool EnvironmentExists(Guid id)
        {
            return _context.Environment.Any(e => e.Id == id);
        }
    }
}
