using Microsoft.EntityFrameworkCore;
using Midterm_EquipmentRental_Team5.Domain.Entities;
using Midterm_EquipmentRental_Team5.Infrastructure.Persistence;
using Midterm_EquipmentRental_Team5.Infrastructure.Repositories.Interfaces;

namespace Midterm_EquipmentRental_Team5.Infrastructure.Repositories
{
    public class ChatRepository(AppDbContext context) : IChatRepository
    {
        private readonly AppDbContext _context = context;

        public List<Message> GetMessagesAsync(int userA, int userB)
        {
            return _context.Messages
                .Where(m => (m.SenderId == userA && m.ReceiverId == userB) ||
                            (m.SenderId == userB && m.ReceiverId == userA))
                .OrderBy(m => m.Timestamp)
                .ToList();
        }

        public Message AddMessageAsync(Message message)
        {
            _context.Messages.Add(message);
            _context.SaveChanges();
            return message;
        }

        public Message? GetMessageByIdAsync(int id)
        {
            return _context.Messages.Find(id);
        }

        public bool DeleteMessageAsync(int id)
        {
            var message = _context.Messages.Find(id);

            if (message == null)
                return false;

            _context.Messages.Remove(message);
            _context.SaveChanges();

            return true;
        }

        public void MarkMessagesAsReadAsync(int senderId, int receiverId)
        {
            var unreadMessages = _context.Messages
                .Where(m => m.SenderId == senderId &&
                            m.ReceiverId == receiverId &&
                            !m.IsRead)
                .ToList();

            foreach (var msg in unreadMessages)
                msg.IsRead = true;

            _context.SaveChanges();
        }

        public List<Chat> GetChatsForUser(int userId)
        {
            return _context.Chat
                .Where(c => c.SenderId == userId || c.ReceiverId == userId)
                .Include(c => c.Sender)
                .Include(c => c.Receiver)
                .ToList();
        }

        public List<Message> GetChatHistory(int chatId, int userId)
        {
            return _context.Messages
                .Where(m => m.ChatId == chatId)
                .OrderBy(m => m.Timestamp)
                .ToList();
        }

        public void EnsureChatExistsAsync(int userA, int userB)
        {
            bool hasAnyMessages = _context.Messages
                .Any(m =>
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

                _context.SaveChanges();
            }
        }
    }
}
