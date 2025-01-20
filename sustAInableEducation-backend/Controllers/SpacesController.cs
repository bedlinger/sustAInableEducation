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
using Space = sustAInableEducation_backend.Models.Space;

namespace sustAInableEducation_backend.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class SpacesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly string _userId;
        private readonly ApplicationUser _user;

        public SpacesController(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _userId = httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            _user = _context.Users.Find(_userId)!;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Space>>> GetSpaces()
        {
            return await _context.SpaceParticipant
                .Where(p => p.UserId == _userId)
                .Include(p => p.Space.Participants)
                .ThenInclude(p => p.User)
                .Include(p => p.Space.Story)
                .Select(p => p.Space)
                .ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Space>> GetSpace(Guid id)
        {
            var space = await _context.SpaceWithAll.FirstOrDefaultAsync(e => e.Id == id);

            if (space == null)
            {
                return NotFound();
            }
            if (!await _context.IsParticipant(_userId, id))
            {
                return Unauthorized();
            }

            return space;
        }

        [HttpPost]
        public async Task<ActionResult<Space>> PostSpace(SpaceRequest spaceReq)
        {
            var space = new Space()
            {
                Participants = new List<SpaceParticipant>()
                {
                    new SpaceParticipant()
                    {
                        UserId = _userId,
                        User = _user,
                        IsHost = true
                    }
                },
                Story = new Story()
                {
                    Topic = spaceReq.Story.Topic,
                    Length = spaceReq.Story.Length,
                    Temperature = spaceReq.Story.Temperature,
                    TopP = spaceReq.Story.TopP,
                    TargetGroup = spaceReq.Story.TargetGroup,
                },
                VotingTimeSeconds = spaceReq.VotingTimeSeconds
            };
            _context.Space.Add(space);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetSpace", new { id = space.Id }, space);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSpace(Guid id)
        {
            var space = await _context.Space.FindAsync(id);
            if (space == null)
            {
                return NotFound();
            }
            if (!await _context.IsHost(_userId, id))
            {
                return Unauthorized();
            }

            _context.Space.Remove(space);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPost("{id}/open")]
        public async Task<ActionResult<SpaceAccessCode>> OpenSpace(Guid id)
        {
            var space = await _context.Space.Include(e => e.AccessCode).FirstOrDefaultAsync(e => e.Id == id);
            if (space == null)
            {
                return NotFound();
            }
            if (!await _context.IsHost(_userId, id))
            {
                return Unauthorized();
            }
            if (space.AccessCode != null)
            {
                _context.SpaceAccessCode.Remove(space.AccessCode);
            }
            string uniqueCode = new Random().Next(0, 1000000).ToString("D6");
            while (await _context.SpaceAccessCode.AnyAsync(p => p.Code == uniqueCode))
            {
                uniqueCode = new Random().Next(0, 1000000).ToString("D6");
            }
            var code = new SpaceAccessCode()
            {
                SpaceId = id,
                Code = uniqueCode
            };
            _context.SpaceAccessCode.Add(code);
            await _context.SaveChangesAsync();
            return code;
        }

        [HttpPost("join")]
        public async Task<ActionResult<Space>> JoinSpace(SpaceAccessCodeRequest accessCode)
        {
            var space = await _context.SpaceWithAll
                .Include(e => e.AccessCode)
                .FirstOrDefaultAsync(e => e.AccessCode != null && e.AccessCode.Code == accessCode.Code);
            if (space == null || space.AccessCode!.ExpiresAt < DateTime.Now)
            {
                return NotFound();
            }
            if (await _context.IsParticipant(_userId, space.Id))
            {
                return Conflict();
            }
            space.Participants.Add(new SpaceParticipant()
            {
                UserId = _userId,
                User = _user,
                SpaceId = space.Id
            });
            await _context.SaveChangesAsync();
            return space;
        }

        [HttpPost("{id}/leave")]
        public async Task<IActionResult> LeaveSpace(Guid id)
        {
            var space = await _context.Space.Include(e => e.Participants).FirstOrDefaultAsync(e => e.Id == id);
            if (space == null)
            {
                return NotFound();
            }
            if (!await _context.IsParticipant(_userId, id))
            {
                return Unauthorized();
            }
            var participant = space.Participants.FirstOrDefault(p => p.UserId == _userId);
            if (participant == null)
            {
                return NotFound();
            }
            space.Participants.Remove(participant);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
