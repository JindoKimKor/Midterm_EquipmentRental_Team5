using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using GreenRockRental_Api.Application.Services.Interfaces;
using GreenRockRental_Api.Application.DTOs;

namespace GreenRockRental_Api.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ChatController(IChatService chatService) : ControllerBase
    {
        private readonly IChatService _chatService = chatService;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ChatListDto>>> GetUserChats()
        {
            var userIdClaim = (User.FindFirst(ClaimTypes.NameIdentifier)?.Value) ?? throw new UnauthorizedAccessException("The user does not have a NameIdentifier claim.");

            if (!int.TryParse(userIdClaim, out int userId))
            {
                throw new FormatException("NameIdentifier claim is not a valid integer.");
            }

            var chats = await _chatService.GetUserChatList(userId);
            var dtos = chats.Select(c => new ChatListDto
            {
                ChatId = c.ChatId,
                SenderId = c.SenderId,
                SenderName = c.Sender?.UserName ?? "Unknown",
                ReceiverId = c.ReceiverId,
                ReceiverName = c.Receiver?.UserName ?? "Unknown",
                LastMessage = "", // Would need to fetch from messages table
                LastMessageTime = DateTime.UtcNow, // Would need to fetch from messages table
                UnreadCount = 0 // Would need to calculate from messages
            });
            return Ok(dtos);
        }

        [HttpGet("{chatId}")]
        public async Task<ActionResult<ChatDetailDto>> GetChatHistory(int chatId)
        {
            var userIdClaim = (User.FindFirst(ClaimTypes.NameIdentifier)?.Value) ?? throw new UnauthorizedAccessException("The user does not have a NameIdentifier claim.");

            if (!int.TryParse(userIdClaim, out int userId))
            {
                throw new FormatException("NameIdentifier claim is not a valid integer.");
            }

            var messages = await _chatService.GetChatHistory(chatId, userId);

            // Get chat info (assuming first message has sender and receiver info)
            var firstMessage = messages.FirstOrDefault();

            var dto = new ChatDetailDto
            {
                ChatId = chatId,
                SenderId = firstMessage?.SenderId ?? userId,
                SenderName = firstMessage?.Sender?.UserName ?? "Unknown",
                ReceiverId = firstMessage?.ReceiverId ?? 0,
                ReceiverName = firstMessage?.Receiver?.UserName ?? "Unknown",
                Messages = messages.Select(m => new MessageDto
                {
                    Id = m.Id,
                    SenderId = m.SenderId,
                    SenderName = m.Sender?.UserName ?? "Unknown",
                    ReceiverId = m.ReceiverId,
                    Content = m.Content,
                    Timestamp = m.Timestamp,
                    IsRead = m.IsRead,
                    ChatId = m.ChatId
                }).ToList()
            };

            return Ok(dto);
        }
    }
}