using Midterm_EquipmentRental_Team5.Domain.Entities;

namespace Midterm_EquipmentRental_Team5.Infrastructure.Repositories.Interfaces
{
    public interface IChatRepository
    {
        List<Message> GetMessagesAsync(int userA, int userB);

        Message AddMessageAsync(Message message);

        Message? GetMessageByIdAsync(int id);

        bool DeleteMessageAsync(int id);

        void MarkMessagesAsReadAsync(int senderId, int receiverId);

        List<Chat> GetChatsForUser(int userId);

        List<Message> GetChatHistory(int chatId, int userId);
        void EnsureChatExistsAsync(int userA, int userB);
    }
}