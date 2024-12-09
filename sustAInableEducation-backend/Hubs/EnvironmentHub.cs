using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using SignalRSwaggerGen.Attributes;
using sustAInableEducation_backend.Models;
using sustAInableEducation_backend.Repository;
using System.Security.Claims;

namespace sustAInableEducation_backend.Hubs
{
    public static class MessageType
    {
        public const string UserJoined = "UserJoined";
        public const string UserLeft = "UserLeft";
        public const string PartGenerating = "PartGenerating";
        public const string PartGenerated = "PartGenerated";
        public const string StoryCompleted = "StoryCompleted";
        public const string VotingStarted = "VotingStarted";
        public const string VotingUpdated = "VotingUpdated";
        public const string ChoiceSet = "ChoiceSet";
    }

    [SignalRHub("environmentHub/{environmentId}", tag: "(WebSocket) EnvironmentHub")]
    [Authorize]
    public class EnvironmentHub : Hub
    {
        private readonly ApplicationDbContext _context;
        private readonly string _userId;
        private readonly Guid _environmentId;
        private readonly IAIService _ai;

        public EnvironmentHub(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor, IAIService ai)
        {
            _context = context;
            _userId = httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier)!;

            Guid.TryParse(httpContextAccessor.HttpContext?.GetRouteValue("id") as string, out _environmentId);

            _ai = ai;
        }

        public override async Task OnConnectedAsync()
        {
            if (!await _context.IsParticipant(_userId, _environmentId))
            {
                throw new HubException("Unauthorized");
            }

            await Groups.AddToGroupAsync(Context!.ConnectionId, _environmentId.ToString());
            _context.EnvironmentParticipant.Find(_environmentId, _userId)!.IsOnline = true;
            await _context.SaveChangesAsync();

            await SendMessage(MessageType.UserJoined, _userId);
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            _context.EnvironmentParticipant.Find(_environmentId, _userId)!.IsOnline = false;
            await _context.SaveChangesAsync();

            await SendMessage(MessageType.UserLeft, _userId);
            await base.OnDisconnectedAsync(exception);
        }

        private async Task SendMessage(string type, object? message = null)
        {
            await Clients.Group(_environmentId.ToString()).SendAsync(type, message);
        }

        public async Task GeneratePart()
        {
            if (!await _context.IsHost(_userId, _environmentId))
            {
                throw new HubException("Unauthorized");
            }
            var story = (await _context.EnvironmentWithStory.FirstOrDefaultAsync(e => e.Id == _environmentId))!.Story;
            if (story.IsComplete)
            {
                throw new HubException("Story is already complete");
            }
            var lastPart = story.Parts.LastOrDefault();
            if (lastPart != null && lastPart.ChosenNumber == null)
            {
                throw new HubException("Choice not set");
            }

            await SendMessage(MessageType.PartGenerating);

            StoryPart part;
            if (story.Parts.Count == 0)
            {
                part = await _ai.StartStory(story);
            }
            else if (story.Parts.Count >= story.Length)
            {
                part = await _ai.GenerateResult(story);
            }
            else
            {
                part = await _ai.GenerateNextPart(story);
            }

            story.Parts.Add(part);
            await _context.SaveChangesAsync();
            if (story.IsComplete)
            {
                await SendMessage(MessageType.StoryCompleted, part);
            }
            else
            {
                await SendMessage(MessageType.PartGenerated, part);
            }
        }

        public async Task StartVoting()
        {
            if (!await _context.IsHost(_userId, _environmentId))
            {
                throw new HubException("Unauthorized");
            }
            var environment = (await _context.EnvironmentWithStory.FirstOrDefaultAsync(e => e.Id == _environmentId))!;
            if (environment.Story.IsComplete)
            {
                throw new HubException("Story is complete");
            }
            var part = environment.Story.Parts.LastOrDefault();
            if (part == null)
            {
                throw new HubException("No part")
;           }
            if (part.IsVotingActive)
            {
                throw new HubException("Voting is already active");
            }
            
            _context.EnvironmentParticipant.Where(p => p.EnvironmentId == _environmentId)
                .ExecuteUpdate(p => p.SetProperty(x => x.HasVoted, x => false));
            foreach (var choice in part.Choices)
            {
                choice.NumberVotes = 0;
            }
            part.VotingEndAt = DateTime.Now.AddSeconds(environment.VotingTimeSeconds);
            await _context.SaveChangesAsync();
            await SendMessage(MessageType.VotingStarted, part.VotingEndAt);
        }

        public async Task Vote(uint number)
        {
            var part = (await _context.EnvironmentWithStory.FirstOrDefaultAsync(e => e.Id == _environmentId))!.Story.Parts.LastOrDefault();
            if (part == null)
            {
                throw new HubException("No part");
            }
            if (!part.IsVotingActive)
            {
                throw new HubException("Voting is not active");
            }
            var participant = (await _context.EnvironmentParticipant.FindAsync(_environmentId, _userId))!;
            if (participant.HasVoted)
            {
                throw new HubException("Already voted");
            }
            part.Choices.First(c => c.Number == number).NumberVotes++;
            participant.HasVoted = true;
            await _context.SaveChangesAsync();
            await SendMessage(MessageType.VotingUpdated, part.Choices);
        }

        public async Task SetChoice(uint number)
        {
            if (!await _context.IsHost(_userId, _environmentId))
            {
                throw new HubException("Unauthorized");
            }
            var part = (await _context.EnvironmentWithStory.FirstOrDefaultAsync(e => e.Id == _environmentId))!.Story.Parts.LastOrDefault();
            if (part == null)
            {
                throw new HubException("No part");
            }
            if (part.ChosenNumber != null)
            {
                throw new HubException("Already chosen");
            }
            if (!part.Choices.Any(c => c.Number == number))
            {
                throw new HubException("Invalid choice");
            }
            part.ChosenNumber = number;
            await _context.SaveChangesAsync();
            await SendMessage(MessageType.ChoiceSet, number);
        }
    }
}
