using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Midterm_EquipmentRental_Team5.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ChatController
    {
        private readonly IChatService _chatService;

        public ChatController(IChatService chatService)
        {
            _chatService = chatService;
        }

        // GET: api/chat
        [HttpGet]
        public async Task<IActionResult> GetChats()
        {
            var chats = await _chatService.GetChatListAsync();
            return Ok(chats);
        }

        // GET: api/chat/{chatId}/messages
        [HttpGet("{chatId}/messages")]
        public async Task<IActionResult> GetMessages(Guid chatId)
        {
            var messages = await _chatService.GetMessagesAsync(chatId);
            return Ok(messages);
        }

        // POST: api/chat/{chatId}/message
        [HttpPost("{chatId}/message")]
        public async Task<IActionResult> SendMessage(Guid chatId, [FromBody] SendMessageDto dto)
        {
            var message = await _chatService.SendMessageAsync(chatId, dto);
            return Ok(message);
        }

    }
}