using Microsoft.EntityFrameworkCore;
using GreenRockRental_Api.Domain.Entities;
using GreenRockRental_Api.Infrastructure.Persistence;
using GreenRockRental_Api.Infrastructure.Repositories.Interfaces;

namespace GreenRockRental_Api.Infrastructure.Repositories
{
    public class ChatRepository(AppDbContext context) : IChatRepository
    {
        private readonly AppDbContext _context = context;

        public async Task<List<Message>> GetMessages(int userA, int userB)
        {
            return await _context.Messages
                .Where(m => (m.SenderId == userA && m.ReceiverId == userB) ||
                            (m.SenderId == userB && m.ReceiverId == userA))
                .OrderBy(m => m.Timestamp)
                .ToListAsync();
        }

        public async Task<Message> SaveMessage(Message message)
        {
            _context.Messages.Add(message);
            await _context.SaveChangesAsync();
            return message;
        }

        public async Task<Message?> GetMessageById(int id)
        {
            return await _context.Messages.FindAsync(id);
        }

        public async Task<bool> DeleteMessage(int id)
        {
            var message = await _context.Messages.FindAsync(id);

            if (message == null)
                return false;

            _context.Messages.Remove(message);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task ClearAllMessages()
        {
            var messages = await _context.Messages.ToListAsync();
            _context.Messages.RemoveRange(messages);
            await _context.SaveChangesAsync();
        }

        public async Task MarkMessagesAsRead(int senderId, int receiverId)
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

        public async Task<List<Chat>> GetChatsForUser(int userId)
        {
            return await _context.Chat
                .Where(c => c.SenderId == userId || c.ReceiverId == userId)
                .Include(c => c.Sender)
                .Include(c => c.Receiver)
                .ToListAsync();
        }

        public async Task<List<Message>> GetChatHistory(int chatId, int userId)
        {
            return await _context.Messages
                .Where(m => m.ChatId == chatId)
                .Include(m => m.Sender)
                .Include(m => m.Receiver)
                .OrderBy(m => m.Timestamp)
                .ToListAsync();
        }

        public async Task EnsureChatExists(int userA, int userB)
        {
            bool hasAnyMessages = await _context.Messages
                .AnyAsync(m =>
                    (m.SenderId == userA && m.ReceiverId == userB) ||
                    (m.SenderId == userB && m.ReceiverId == userA)
                );

            if (!hasAnyMessages)
            {
                _context.Messages.Add(new Message
                {
                    SenderId = userA,
                    ReceiverId = userB,
                    Content = "(new chat started)",
                    Timestamp = DateTime.UtcNow,
                    IsRead = true
                });

                await _context.SaveChangesAsync();
            }
        }
    }
}
