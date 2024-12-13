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

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Environment>>> GetEnvironments()
        {
            return await _context.EnvironmentParticipant
                .Where(p => p.UserId == _userId)
                .Include(p => p.Environment.Participants)
                .ThenInclude(p => p.User)
                .Include(p => p.Environment.Story)
                .Select(p => p.Environment)
                .ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Environment>> GetEnvironment(Guid id)
        {
            var environment = await _context.EnvironmentWithAll.FirstOrDefaultAsync(e => e.Id == id);

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

        [HttpPost]
        public async Task<ActionResult<Environment>> PostEnvironment(Environment environment)
        {
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
        public async Task<ActionResult<EnvironmentAccessCode>> OpenEnvironment(Guid id)
        {
            var environment = await _context.Environment.Include(e => e.AccessCode).FirstOrDefaultAsync(e => e.Id == id);
            if (environment == null)
            {
                return NotFound();
            }
            if (!await _context.IsHost(_userId, id))
            {
                return Unauthorized();
            }
            if (environment.AccessCode != null)
            {
                _context.EnvironmentAccessCode.Remove(environment.AccessCode);
            }
            string uniqueCode = new Random().Next(0, 1000000).ToString("D6");
            while (await _context.EnvironmentAccessCode.AnyAsync(p => p.Code == uniqueCode))
            {
                uniqueCode = new Random().Next(0, 1000000).ToString("D6");
            }
            var code = new EnvironmentAccessCode()
            {
                EnvironmentId = id,
                Code = uniqueCode
            };
            _context.EnvironmentAccessCode.Add(code);
            await _context.SaveChangesAsync();
            return code;
        }

        [HttpPost("join")]
        public async Task<ActionResult<Environment>> JoinEnvironment(EnvironmentAccessCodeRequest accessCode)
        {
            var environment = await _context.EnvironmentWithAll
                .Include(e => e.AccessCode)
                .FirstOrDefaultAsync(e => e.AccessCode != null && e.AccessCode.Code == accessCode.Code);
            if (environment == null || environment.AccessCode!.ExpiresAt < DateTime.Now)
            {
                return NotFound();
            }
            if (await _context.IsParticipant(_userId, environment.Id))
            {
                return Conflict();
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
