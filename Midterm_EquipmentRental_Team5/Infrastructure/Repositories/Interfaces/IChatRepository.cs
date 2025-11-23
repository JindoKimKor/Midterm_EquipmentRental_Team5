using Midterm_EquipmentRental_Team5.Domain.Entities;

namespace Midterm_EquipmentRental_Team5.Infrastructure.Repositories.Interfaces
{
    public interface IChatRepository
    {
        List<Message> GetMessagesAsync(string userA, string userB);

        Message AddMessageAsync(Message message);

        Message? GetMessageByIdAsync(int id);

        bool DeleteMessageAsync(int id);

        void MarkMessagesAsReadAsync(string senderId, string receiverId);

        List<Chat> GetChatsForUser(int userId);

        List<Message> GetChatHistory(int chatId, int userId);
        void EnsureChatExistsAsync(string userA, string userB);
    }
}