using Midterm_EquipmentRental_Team5.Application.Services.Interfaces;
using Midterm_EquipmentRental_Team5.Domain.Entities;
using Midterm_EquipmentRental_Team5.Infrastructure.Repositories.Interfaces;

namespace Midterm_EquipmentRental_Team5.Application.Services
{
    public class ChatService(IChatRepository chatRepo) : IChatService
    {
        private readonly IChatRepository _chatRepo = chatRepo;

        private async Task<bool> CheckIfUserIsPartOfChat(int chatId, int userId)
        {
            var chats = await _chatRepo.GetChatsForUser(userId);
            return chats.Any(c => c.ChatId == chatId);
        }

        public async Task<IEnumerable<Chat>> GetUserChatList(int userId)
        {
            var chats = await _chatRepo.GetChatsForUser(userId);
            return chats;
        }
        public async Task<IEnumerable<Message>> GetChatHistory(int chatId, int userId)
        {
            if (await CheckIfUserIsPartOfChat(chatId, userId))
            {
                var message = await _chatRepo.GetChatHistory(chatId, userId);
                return message;
            }
            return [];
        }

        public async Task ClearMessageFromDb()
        {
            await _chatRepo.ClearAllMessages();
        }

        public async Task AddMessage(Message message)
        {
            await _chatRepo.SaveMessage(message);
        }
    }
}