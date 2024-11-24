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
        private readonly string _userId;
        private readonly ApplicationUser _user;

        public EnvironmentsController(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _userId = httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            _user = _context.Users.Find(_userId)!;
        }

        // GET: api/Environments
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Environment>>> GetEnvironment()
        {
            return await _context.EnvironmentParticipant
                .Where(p => p.UserId == _userId)
                .Include(p => p.Environment.Participants)
                .Select(p => p.Environment)
                .ToListAsync();
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
            if (!await _context.IsParticipant(_userId, id))
            {
                return Unauthorized();
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
            if ((environment.Story.PresetId == null || !await _context.StoryPreset.AnyAsync(p => p.Id == environment.Story.PresetId)) && 
                (environment.Story.Prompt == null || environment.Story.Length == null || environment.Story.Creativity == null))
            {
                return BadRequest();
            }
            environment.Participants = new List<EnvironmentParticipant>()
            {
                new EnvironmentParticipant()
                {
                    UserId = _userId,
                    User = _user,
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
            if (!await _context.IsHost(_userId, id))
            {
                return Unauthorized();
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
