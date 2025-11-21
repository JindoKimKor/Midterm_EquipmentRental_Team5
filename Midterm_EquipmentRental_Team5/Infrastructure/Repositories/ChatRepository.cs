using Microsoft.EntityFrameworkCore;
using Midterm_EquipmentRental_Team5.Domain.Entities;
using Midterm_EquipmentRental_Team5.Infrastructure.Persistence;

namespace Midterm_EquipmentRental_Team5.Infrastructure.Repositories
{
    public class ChatRepository(AppDbContext context)
    {
        private readonly AppDbContext _context = context;

        public async Task<List<Message>> GetMessagesAsync(string userA, string userB)
        {
            return await _context.Messages
                .Where(m => (m.SenderId == userA && m.ReceiverId == userB) ||
                            (m.SenderId == userB && m.ReceiverId == userA))
                .OrderBy(m => m.Timestamp)
                .ToListAsync();
        }

        // ✔ Add a new message
        public async Task<Message> AddMessageAsync(Message message)
        {
            _context.Messages.Add(message);
            await _context.SaveChangesAsync();
            return message;
        }

        // ✔ Get a single message by ID
        public async Task<Message?> GetMessageByIdAsync(int id)
        {
            return await _context.Messages.FindAsync(id);
        }

        // ✔ Delete a message
        public async Task<bool> DeleteMessageAsync(int id)
        {
            var message = await _context.Messages.FindAsync(id);

            if (message == null)
                return false;

            _context.Messages.Remove(message);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task MarkMessagesAsReadAsync(string senderId, string receiverId)
        {
            var unreadMessages = await _context.Messages
                .Where(m => m.SenderId == senderId &&
                            m.ReceiverId == receiverId &&
                            !m.IsRead)
                .ToListAsync();

            foreach (var msg in unreadMessages)
                msg.IsRead = true;

            await _context.SaveChangesAsync();
        }


        public async Task<List<ChatPreview>> GetChatsForUserAsync(string userId)
        {
            var chats = await _context.Messages
                .Where(m => m.SenderId == userId || m.ReceiverId == userId)
                .GroupBy(m => m.SenderId == userId ? m.ReceiverId : m.SenderId)
                .Select(g => new ChatPreview
                {
                    UserId = g.Key,
                    LastMessage = g.OrderByDescending(m => m.Timestamp).First().Content,
                    LastMessageTime = g.OrderByDescending(m => m.Timestamp).First().Timestamp,
                    UnreadCount = g.Count(m => m.ReceiverId == userId && !m.IsRead)
                })
                .OrderByDescending(c => c.LastMessageTime)
                .ToListAsync();

            return chats;
        }


        public async Task<List<Message>> GetMessagesAsync(string userA, string userB)
        {
            return await _context.Messages
                .Where(m => (m.SenderId == userA && m.ReceiverId == userB) ||
                            (m.SenderId == userB && m.ReceiverId == userA))
                .OrderBy(m => m.Timestamp)
                .ToListAsync();
        }

        public async Task<Message> AddMessageAsync(Message message)
        {
            _context.Messages.Add(message);
            await _context.SaveChangesAsync();
            return message;
        }

        public async Task<Message?> GetMessageByIdAsync(int id)
        {
            return await _context.Messages.FindAsync(id);
        }

        public async Task<bool> DeleteMessageAsync(int id)
        {
            var message = await _context.Messages.FindAsync(id);

            if (message == null)
                return false;

            _context.Messages.Remove(message);
            await _context.SaveChangesAsync();

            return true;
        }


        public async Task MarkMessagesAsReadAsync(string senderId, string receiverId)
        {
            var unreadMessages = await _context.Messages
                .Where(m => m.SenderId == senderId &&
                            m.ReceiverId == receiverId &&
                            !m.IsRead)
                .ToListAsync();

            foreach (var msg in unreadMessages)
                msg.IsRead = true;

            await _context.SaveChangesAsync();
        }

        public async Task EnsureChatExistsAsync(string userA, string userB)
        {
            bool hasAnyMessages = await _context.Messages
                .AnyAsync(m =>
                    (m.SenderId == userA && m.ReceiverId == userB) ||
                    (m.SenderId == userB && m.ReceiverId == userA)
                );

            // If no chat exists, add an empty "placeholder"
            if (!hasAnyMessages)
            {
                _context.Messages.Add(new Message
                {
                    SenderId = userA,
                    ReceiverId = userB,
                    Content = "(new chat started)", // you may hide this message on frontend
                    Timestamp = DateTime.UtcNow,
                    IsRead = true
                });

                await _context.SaveChangesAsync();
            }
        }
    }
}
