using System.Security.Claims;
using Luvora.Application.Interfaces.Services;
using Luvora.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Luvora.API.Controllers.DiscoveryProfile
{
    [Route("api/discoverprofiles")]
    [ApiController]
    [Authorize]
    public class DiscoveryProfileController : ControllerBase
    {
        private readonly IDiscoveryProfileService _discoveryProfile;

        public DiscoveryProfileController(IDiscoveryProfileService discoveryProfile)
        {
            _discoveryProfile = discoveryProfile;
        }

        [HttpGet]
        public async Task<IActionResult> GetProfiles()
        {
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var profiles = await _discoveryProfile.GetDiscoveryProfilesAsync(userId);

            return Ok(profiles);
        }
    }
}
