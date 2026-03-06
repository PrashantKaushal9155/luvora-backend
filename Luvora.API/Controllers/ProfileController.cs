using System.Security.Claims;
using Luvora.Application.DTOs.Profile;
using Luvora.Application.Interfaces;
using Luvora.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Luvora.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ProfileController : ControllerBase
    {
        private readonly IUserProfileRepository _profileRepository;

        public ProfileController(IUserProfileRepository profileRepository)
        {
            _profileRepository = profileRepository;
        }

        [HttpPost("upsert")]
        public async Task<IActionResult> UpsertProfile(UpsertProfileRequestDto request)
        {
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var profile = new UserProfile(
                userId,
                request.Name,
                request.DateOfBirth,
                request.Gender,
                request.PreferredGender,
                request.RelationshipStatus,
                request.MinPreferredAge,
                request.MaxPreferredAge,
                request.City,
                request.Bio,
                request.Country,
                request.Occupation
                );

            var result = await _profileRepository.UpsertAsync(profile);

            if (!result)
                return BadRequest("Profile update failed!");

            return Ok("Profile saved successfully.");
        }

        [HttpGet]
        public async Task<IActionResult> GetProfile()
        {
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var profile = await _profileRepository.GetByUserIdAsync(userId);

            if (profile == null)
                return NotFound("Profile not created yet!");

            return Ok(profile);
        }
    }
}
