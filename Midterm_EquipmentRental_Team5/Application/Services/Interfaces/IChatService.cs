using Midterm_EquipmentRental_Team5.Domain.Entities;

namespace Midterm_EquipmentRental_Team5.Application.Services.Interfaces
{
    public interface IChatService
    {
        IEnumerable<Chat> GetUserChatList(int userId);
        IEnumerable<Message> GetChatHistory(int chatId, int userId);
        void AddMessage(Message message);
        void ClearMessageFromDb();
    }
}