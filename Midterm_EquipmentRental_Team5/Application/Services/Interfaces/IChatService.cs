using Midterm_EquipmentRental_Team5.Domain.Entities;

namespace Midterm_EquipmentRental_Team5.Application.Services.Interfaces
{
    public interface IChatService
    {
        Task<IEnumerable<Chat>> GetUserChatList(int userId);
        Task<IEnumerable<Message>> GetChatHistory(int chatId, int userId);
        Task AddMessage(Message message);
        Task ClearMessageFromDb();
    }
}