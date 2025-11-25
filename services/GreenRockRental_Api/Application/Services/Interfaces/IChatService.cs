using GreenRockRental_Api.Domain.Entities;

namespace GreenRockRental_Api.Application.Services.Interfaces
{
    public interface IChatService
    {
        Task<IEnumerable<Chat>> GetUserChatList(int userId);
        Task<IEnumerable<Message>> GetChatHistory(int chatId, int userId);
        Task AddMessage(Message message);
        Task ClearMessageFromDb();
    }
}