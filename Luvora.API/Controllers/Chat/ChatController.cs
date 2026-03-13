using System.Security.Claims;
using Luvora.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Luvora.API.Controllers.Chat
{
    [Route("api/chat")]
    [ApiController]
    [Authorize]
    public class ChatController : ControllerBase
    {
        private readonly IChatService _chatService;

        public ChatController(IChatService chatService)
        {
            _chatService = chatService;
        }

        [HttpGet("{matchId}")]
        public async Task<IActionResult> GetMessages(Guid matchId)
        {
            var messages = await _chatService.GetMessagesAsync(matchId);

            return Ok(messages);
        }

        [HttpPost("mark-read/{matchId}")]
        public async Task<IActionResult> MarkRead(Guid matchId)
        {
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            await _chatService.MarkMessagesReadAsync(matchId, userId);

            return Ok();
        }
    }
}
