using System.Security.Claims;
using Luvora.Application.DTOs.Swipe;
using Luvora.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Luvora.API.Controllers.Swipe
{
    [Route("api/swipe")]
    [ApiController]
    [Authorize]
    public class SwipeController : ControllerBase
    {
        private readonly ISwipeService _swipeService;

        public SwipeController(ISwipeService swipeService)
        {
            _swipeService = swipeService;
        }

        [HttpPost]
        public async Task<IActionResult> Swipe([FromBody] SwipeRequestDto request)
        {
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var result = await _swipeService.SwipeAsync(userId, request);

            return Ok(result);
        }
    }
}
