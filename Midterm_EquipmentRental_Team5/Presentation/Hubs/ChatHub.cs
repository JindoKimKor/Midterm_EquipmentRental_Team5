using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;

namespace Midterm_EquipmentRental_Team5.Presentation.Hubs
{
    [Authorize]
    public class ChatHub : Hub
    {
        private static readonly ConcurrentDictionary<string, (string Username, string Room)> _users = new();

        public async Task SendPrivateMessage(string receiverId, string message)
        {
            var senderId = Context.UserIdentifier;

            await Clients.User(receiverId).SendAsync("ReceiveMessage", new
            {
                senderId,
                message,
                timestamp = DateTime.UtcNow
            });
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            if (_users.TryRemove(Context.ConnectionId, out var userInfo))
            {
                await Clients.Group(userInfo.Room).SendAsync("UserDisconnected", new
                {
                    username = userInfo.Username,
                    connectionId = Context.ConnectionId,
                    timestamp = DateTime.UtcNow,
                    message = $"{userInfo.Username} disconnected"
                });
            }

            await base.OnDisconnectedAsync(exception);
        }
    }
}