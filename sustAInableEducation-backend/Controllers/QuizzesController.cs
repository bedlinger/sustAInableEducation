using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using sustAInableEducation_backend.Models;
using sustAInableEducation_backend.Repository;

namespace sustAInableEducation_backend.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class QuizzesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly string _userId;
        private readonly IAIService _ai;

        public QuizzesController(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor, IAIService ai)
        {
            _context = context;
            _userId = httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            _ai = ai;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Quiz>>> GetQuizzes(Guid? environmentId)
        {
            if (environmentId != null)
            {
                return await _context.Quiz.Where(q => q.UserId == _userId && q.EnvironmentId == environmentId).ToListAsync();
            }
            return await _context.Quiz.Where(q => q.UserId == _userId).ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Quiz>> GetQuiz(Guid id)
        {
            var quiz = await _context.Quiz.FindAsync(id);

            if (quiz == null)
            {
                return NotFound();
            }
            if (quiz.UserId != _userId)
            {
                return Unauthorized();
            }

            return quiz;
        }

        [HttpPost]
        public async Task<ActionResult<Quiz>> PostQuiz(QuizCreate quizCreate)
        {
            if (!await _context.IsParticipant(_userId, quizCreate.EnvironmentId))
            {
                return Unauthorized();
            }
            var story = (await _context.EnvironmentWithStory.FirstOrDefaultAsync(e => e.Id == quizCreate.EnvironmentId))!.Story;
            var quiz = await _ai.GenerateQuiz(story, quizCreate.Types);
            quiz.UserId = _userId;
            quiz.EnvironmentId = quizCreate.EnvironmentId;
            _context.Quiz.Add(quiz);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetQuiz", new { id = quiz.Id }, quiz);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteQuiz(Guid id)
        {
            var quiz = await _context.Quiz.FindAsync(id);
            if (quiz == null)
            {
                return NotFound();
            }
            if(quiz.UserId != _userId)
            {
                return Unauthorized();
            }

            _context.Quiz.Remove(quiz);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
