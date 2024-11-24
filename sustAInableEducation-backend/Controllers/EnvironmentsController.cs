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
                .ThenInclude(p => p.User)
                .Include(p => p.Environment.Story)
                .Select(p => p.Environment)
                .ToListAsync();
        }

        // GET: api/Environments/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Environment>> GetEnvironment(Guid id)
        {
            var environment = await _context.EnvironmentHydrated.FirstOrDefaultAsync(e => e.Id == id);

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

        [HttpPost("{id}/open")]
        public async Task<ActionResult<EnvironmentPIN>> OpenEnvironment(Guid id)
        {
            var environment = await _context.Environment.Include(e => e.PIN).FirstOrDefaultAsync(e => e.Id == id);
            if (environment == null)
            {
                return NotFound();
            }
            if (!await _context.IsHost(_userId, id))
            {
                return Unauthorized();
            }
            if (environment.PIN != null)
            {
                _context.EnvironmentPIN.Remove(environment.PIN);
            }
            string uniquePIN = new Random().Next(0, 1000000).ToString("D6");
            while (await _context.EnvironmentPIN.AnyAsync(p => p.PIN == uniquePIN))
            {
                uniquePIN = new Random().Next(0, 1000000).ToString("D6");
            }
            var pin = new EnvironmentPIN()
            {
                EnvironmentId = id,
                PIN = uniquePIN
            };
            _context.EnvironmentPIN.Add(pin);
            await _context.SaveChangesAsync();
            return pin;
        }

        [HttpPost("join")]
        public async Task<ActionResult<Environment>> JoinEnvironment(EnvironmentPINRequest pin)
        {
            var environment = await _context.EnvironmentHydrated.Include(e => e.PIN).FirstOrDefaultAsync(e => e.PIN != null && e.PIN.PIN == pin.PIN);
            if (environment == null || environment.PIN!.ExpiresAt < DateTime.Now)
            {
                return NotFound();
            }
            if (await _context.IsParticipant(_userId, environment.Id))
            {
                return BadRequest();
            }
            environment.Participants.Add(new EnvironmentParticipant()
            {
                UserId = _userId,
                User = _user,
                EnvironmentId = environment.Id
            });
            await _context.SaveChangesAsync();
            return environment;
        }

        [HttpPost("{id}/leave")]
        public async Task<IActionResult> LeaveEnvironment(Guid id)
        {
            var environment = await _context.Environment.Include(e => e.Participants).FirstOrDefaultAsync(e => e.Id == id);
            if (environment == null)
            {
                return NotFound();
            }
            if (!await _context.IsParticipant(_userId, id))
            {
                return Unauthorized();
            }
            var participant = environment.Participants.FirstOrDefault(p => p.UserId == _userId);
            if (participant == null)
            {
                return NotFound();
            }
            environment.Participants.Remove(participant);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
