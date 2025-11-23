using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Midterm_EquipmentRental_Team5.Application.Services.Interfaces;
using Midterm_EquipmentRental_Team5.Domain.Entities;
using System.Collections.Concurrent;

namespace Midterm_EquipmentRental_Team5.Presentation.Hubs
{
    [Authorize]
    public class ChatHub(IChatService chatService) : Hub
    {
        private static readonly ConcurrentDictionary<string, HashSet<string>> _onlineUsers = new();
        private readonly IChatService _chatService = chatService;

        public async Task SendMessage(int receiverId, int chatId, string message)
        {
            try
            {
                var senderId = int.Parse(Context.UserIdentifier ?? throw new HubException("Unauthorized user."));

                if (string.IsNullOrWhiteSpace(message))
                    throw new HubException("Message cannot be empty.");

                bool userOnline = IsUserOnline(receiverId.ToString());

                var msg = new Message
                {
                    SenderId = senderId,
                    ReceiverId = receiverId,
                    Content = message,
                    Timestamp = DateTime.UtcNow,
                    ChatId = chatId,
                    IsRead = userOnline
                };

                // Deliver via SignalR only if receiver is online
                if (userOnline)
                {
                    await Clients.User(receiverId.ToString()).SendAsync("ReceiveMessage", senderId, chatId, message, DateTime.UtcNow);
                }

                await Clients.User(senderId.ToString()).SendAsync("ReceiveMessage", senderId, chatId, message, DateTime.UtcNow);

                // Always save to DB
                // await _chatService.AddMessage(msg);
            }
            catch (Exception ex)
            {
                throw new HubException($"Error sending message: {ex.Message}", ex);
            }
        }

        public bool IsUserOnline(string userId)
        {
            return _onlineUsers.ContainsKey(userId);
        }

        public override async Task OnConnectedAsync()
        {
            var userId = Context.UserIdentifier;
            var connectionId = Context.ConnectionId;

            if (userId == null)
                throw new HubException("Unauthorized connection.");

            _onlineUsers.AddOrUpdate(
                userId,
                _ => new HashSet<string> { connectionId },
                (_, existingConnections) =>
                {
                    existingConnections.Add(connectionId);
                    return existingConnections;
                }
            );

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var userId = Context.UserIdentifier;
            var connectionId = Context.ConnectionId;

            if (userId != null &&
                _onlineUsers.TryGetValue(userId, out var connections))
            {
                connections.Remove(connectionId);

                if (connections.Count == 0)
                {
                    _onlineUsers.TryRemove(userId, out _);
                }
            }

            await base.OnDisconnectedAsync(exception);
        }
    }
}
