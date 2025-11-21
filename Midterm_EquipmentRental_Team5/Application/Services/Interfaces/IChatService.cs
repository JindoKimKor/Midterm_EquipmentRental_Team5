using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Midterm_EquipmentRental_Team5.Application.Services.Interfaces
{
    public interface IChatService
    {
        Task<IEnumerable<ChatDto>> GetChatListAsync();

        Task<IEnumerable<MessageDto>> GetMessagesAsync(Guid chatId);

        Task<MessageDto> SendMessageAsync(Guid chatId, SendMessageDto dto);
    }
}