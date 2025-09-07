
using Mafia.Chats.Application.Service;
using Microsoft.AspNetCore.Mvc;
using Mafia.Chats.Application.DTOs;
using Mafia.Games.Domain.Models;
namespace Mafia.Chats.API.Controllers

{
    [ApiController]
    [Route("api/[controller]")]
    public class ChatController : ControllerBase
    {
        private readonly ChatService _chatService;

        public ChatController(ChatService chatService)
        {
            _chatService = chatService;
        }

        [HttpGet("{chatId}")]
        public async Task<IActionResult> GetChat(Guid chatId)
        {
            var chat = await _chatService.GetChatByIdAsync(chatId);
            if (chat == null)
                return NotFound();

            return Ok(chat);
        }

        [HttpPost("{chatId}/messages")]
        public async Task<IActionResult> SendMessage(Guid chatId, [FromBody] ChatMessageDto dto)
        {
            var message = new ChatMessage(dto.SenderId, dto.SenderName, dto.Content, Enum.Parse<ChatType>(dto.Type));

            await _chatService.SendMessage(chatId, message);

            return Ok(message);
        }

        [HttpPost]
        public async Task<IActionResult> CreateChat([FromBody] CreateChatDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.ChatTitle))
                return BadRequest("Название чата не может быть пустым");

            var chat = await _chatService.CreateChatAsync(dto.ChatTitle);

            return CreatedAtAction(nameof(GetChat), new { chatId = chat.Id }, chat);
        }
        
        
    }
}
