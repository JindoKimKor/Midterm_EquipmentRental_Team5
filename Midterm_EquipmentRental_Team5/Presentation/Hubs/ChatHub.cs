using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;

namespace Midterm_EquipmentRental_Team5.Presentation.Hubs
{
    public class ChatHub : Hub
    {
        private static readonly ConcurrentDictionary<string, (string Username, string Room)> _users =
            new ConcurrentDictionary<string, (string, string)>();

        public async Task JoinRoom(string room, string username)
        {
            _users.TryAdd(Context.ConnectionId, (username, room));
            await Groups.AddToGroupAsync(Context.ConnectionId, room);
            await Clients.Group(room).SendAsync("UserJoined", new
            {
                username,
                connectionId = Context.ConnectionId,
                timestamp = DateTime.UtcNow,
                message = $"{username} joined the chat"
            });
        }

        public async Task LeaveRoom(string room, string username)
        {
            _users.TryRemove(Context.ConnectionId, out _);
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, room);
            await Clients.Group(room).SendAsync("UserLeft", new
            {
                username,
                connectionId = Context.ConnectionId,
                timestamp = DateTime.UtcNow,
                message = $"{username} left the chat"
            });
        }

        public async Task SendMessage(string room, string username, string messageText)
        {
            await Clients.Group(room).SendAsync("ReceiveMessage", new
            {
                username,
                connectionId = Context.ConnectionId,
                message = messageText,
                timestamp = DateTime.UtcNow,
                isCurrentUser = false
            });
        }

        public async Task GetOnlineUsers(string room)
        {
            var onlineUsers = _users
                .Where(u => u.Value.Room == room)
                .Select(u => new { username = u.Value.Username, connectionId = u.Key })
                .ToList();

            await Clients.Caller.SendAsync("OnlineUsers", onlineUsers);
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