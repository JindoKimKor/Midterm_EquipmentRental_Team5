using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Midterm_EquipmentRental_Team5.Application.DTOs;
using Midterm_EquipmentRental_Team5.Application.Services.Interfaces;

namespace Midterm_EquipmentRental_Team5.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ChatController(IChatService chatService) : ControllerBase
    {
        private readonly IChatService _chatService = chatService;

        [HttpGet]
        public ActionResult GetUserChats()
        {
            var userIdClaim = (User.FindFirst(ClaimTypes.NameIdentifier)?.Value) ?? throw new UnauthorizedAccessException("The user does not have a NameIdentifier claim.");

            if (!int.TryParse(userIdClaim, out int userId))
            {
                throw new FormatException("NameIdentifier claim is not a valid integer.");
            }

            var chats = _chatService.GetUserChatList(userId);
            return Ok(chats);
        }

        [HttpGet("{chatId}")]
        public ActionResult GetChatHistory(int chatId)
        {
            var userIdClaim = (User.FindFirst(ClaimTypes.NameIdentifier)?.Value) ?? throw new UnauthorizedAccessException("The user does not have a NameIdentifier claim.");

            if (!int.TryParse(userIdClaim, out int userId))
            {
                throw new FormatException("NameIdentifier claim is not a valid integer.");
            }

            var chats = _chatService.GetChatHistory(chatId, userId);
            return Ok(chats);
        }

        // [HttpGet("{chatId}/messages")]
        // public async Task<IActionResult> GetMessages(Guid chatId)
        // {
        //     var messages = await _chatService.GetMessages(chatId);
        //     return Ok(messages);
        // }

        // [HttpPost("{chatId}/message")]
        // public async Task<IActionResult> SendMessage(Guid chatId, [FromBody] SendMessageDto dto)
        // {
        //     var message = await _chatService.SendMessage(chatId, dto);
        //     return Ok(message);
        // }

    }
}