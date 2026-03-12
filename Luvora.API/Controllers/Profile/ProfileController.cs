using System.Security.Claims;
using Luvora.Application.DTOs.Profile;
using Luvora.Application.Interfaces.Repositories;
using Luvora.Application.Interfaces.Services;
using Luvora.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Luvora.API.Controllers.Profile
{
    [Authorize]
    [Route("api/profile")]
    [ApiController]
    public class ProfileController : ControllerBase
    {
        private readonly IProfileService _profileService;

        public ProfileController(IProfileService profileService)
        {
           _profileService = profileService;
        }

        [HttpPost("upsert")]
        public async Task<IActionResult> UpsertProfile(UpsertProfileRequestDto request)
        {
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var result = await _profileService.UpsertProfileAsync(userId, request);

            if (!result)
                return BadRequest("Profile update failed!");

            return Ok("Profile saved successfully.");
        }

        [HttpGet]
        public async Task<IActionResult> GetProfile()
        {
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var profile = await _profileService.GetProfileAsync(userId);

            if (profile == null)
                return NotFound("Profile not created yet!");

            return Ok(profile);
        }
    }
}
