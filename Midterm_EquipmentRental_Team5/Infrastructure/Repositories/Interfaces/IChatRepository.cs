using Midterm_EquipmentRental_Team5.Domain.Entities;

namespace Midterm_EquipmentRental_Team5.Infrastructure.Repositories.Interfaces
{
    public interface IChatRepository
    {
        Task<List<Message>> GetMessages(int userA, int userB);
        Task<Message> SaveMessage(Message message);
        Task<Message?> GetMessageById(int id);
        Task<bool> DeleteMessage(int id);
        Task MarkMessagesAsRead(int senderId, int receiverId);
        Task<List<Chat>> GetChatsForUser(int userId);
        Task<List<Message>> GetChatHistory(int chatId, int userId);
        Task EnsureChatExists(int userA, int userB);
        Task ClearAllMessages();
    }
}