using Midterm_EquipmentRental_Team5.Domain.Entities;

namespace Midterm_EquipmentRental_Team5.Infrastructure.Repositories.Interfaces
{
    public interface IChatRepository
    {
        List<Message> GetMessages(int userA, int userB);

        Message SaveMessage(Message message);

        Message? GetMessageById(int id);

        bool DeleteMessage(int id);

        void MarkMessagesAsRead(int senderId, int receiverId);

        List<Chat> GetChatsForUser(int userId);

        List<Message> GetChatHistory(int chatId, int userId);
        void EnsureChatExists(int userA, int userB);
        void ClearAllMessages();
    }
}