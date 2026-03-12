using System.Security.Claims;
using Luvora.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Luvora.API.Controllers.Match
{
    [Route("api/matches")]
    [ApiController]
    [Authorize]
    public class MatchController : ControllerBase
    {
        private readonly IMatchService _matchService;

        public MatchController(IMatchService matchService)
        {
            _matchService = matchService;
        }

        [HttpGet]
        public async Task<IActionResult> GetMatches()
        {
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var matches = await _matchService.GetUserMatchesAsync(userId);

            return Ok(matches);
        }
    }
}
