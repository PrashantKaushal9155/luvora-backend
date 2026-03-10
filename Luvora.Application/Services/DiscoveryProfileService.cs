using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Luvora.Application.DTOs.DiscoveryProfile;
using Luvora.Application.Interfaces.Repositories;
using Luvora.Application.Interfaces.Services;

namespace Luvora.Application.Services
{
    public class DiscoveryProfileService : IDiscoveryProfileService
    {
        private readonly IUserProfileRepository _profileRepository;

        public DiscoveryProfileService(IUserProfileRepository profileRepository)
        {
            _profileRepository = profileRepository;
        }
        public async Task<IEnumerable<DiscoveryProfileDto>> GetDiscoveryProfilesAsync(Guid userId)
        {
            // get current user profile
            var profile = await _profileRepository.GetByUserIdAsync(userId);

            if (profile == null)
                throw new Exception("Profile not found");

            // fetch discovery profiles
            var profiles = await _profileRepository.GetDiscoveryProfileAsync(
                userId,
                profile.PreferredGender,
                profile.MinPreferredAge,
                profile.MaxPreferredAge
            );

            return profiles;
        }
    }
}
