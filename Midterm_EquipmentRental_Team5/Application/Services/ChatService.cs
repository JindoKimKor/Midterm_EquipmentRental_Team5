using Midterm_EquipmentRental_Team5.Application.DTOs;
using Midterm_EquipmentRental_Team5.Application.Services.Interfaces;
using Midterm_EquipmentRental_Team5.Domain.Entities;
using Midterm_EquipmentRental_Team5.Infrastructure.Repositories.Interfaces;

namespace Midterm_EquipmentRental_Team5.Application.Services
{
    public class ChatService(IChatRepository chatRepo) : IChatService
    {
        private readonly IChatRepository _chatRepo = chatRepo;

        private bool CheckIfUserIsPartOfChat(int chatId, int userId)
        {
            var chats = _chatRepo.GetChatsForUser(userId);
            return chats.Any(c => c.ChatId == chatId);
        }

        public IEnumerable<Chat> GetUserChatList(int userId)
        {
            var chats = _chatRepo.GetChatsForUser(userId);
            return chats;
        }
        public IEnumerable<Message> GetChatHistory(int chatId, int userId)
        {
            if (CheckIfUserIsPartOfChat(chatId, userId))
            {
                var message = _chatRepo.GetChatHistory(chatId, userId);
                return message;
            }
            return [];
        }

        public void ClearMessageFromDb()
        {
            _chatRepo.ClearAllMessages();
        }

        public void AddMessage(Message message)
        {
            _chatRepo.SaveMessage(message);
        }

        public IEnumerable<MessageDto> GetMessages(int chatId)
        {
            throw new NotImplementedException();
        }
    }
}