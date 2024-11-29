using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using SignalRSwaggerGen.Attributes;
using sustAInableEducation_backend.Models;
using sustAInableEducation_backend.Repository;
using System.Security.Claims;
using Environment = sustAInableEducation_backend.Models.Environment;

namespace sustAInableEducation_backend.Hubs
{
    [SignalRHub("environmentHub/{environmentId}", tag: "(WebSocket) EnvironmentHub")]
    [Authorize]
    public class EnvironmentHub : Hub
    {
        private readonly ApplicationDbContext _context;
        private readonly string _userId;
        private readonly ApplicationUser _user;
        private readonly Guid _environmentId;

        public EnvironmentHub(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _userId = httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            _user = _context.Users.Find(_userId)!;

            Guid.TryParse(httpContextAccessor.HttpContext?.GetRouteValue("id") as string, out _environmentId);
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

            await Clients.Group(_environmentId.ToString()).SendAsync("ReceiveMessage", "System", $"{_user.AnonUserName} joined the environment");
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            _context.EnvironmentParticipant.Find(_environmentId, _userId)!.IsOnline = false;
            await _context.SaveChangesAsync();

            await Clients.Group(_environmentId.ToString()).SendAsync("ReceiveMessage", "System", $"{_user.AnonUserName} left the environment");
            await base.OnDisconnectedAsync(exception);
        }

        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }
    }
}
