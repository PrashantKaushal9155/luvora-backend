using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Luvora.Application.DTOs.Profile;
using Luvora.Application.Interfaces.Repositories;
using Luvora.Application.Interfaces.Services;
using Luvora.Domain.Entities;

namespace Luvora.Application.Services
{
    public class ProfileService : IProfileService
    {
        private readonly IUserProfileRepository _profileRepository;

        public ProfileService(IUserProfileRepository profileRepository)
        {
            _profileRepository = profileRepository;
        }

        public async Task<UserProfile?> GetProfileAsync(Guid userId)
        {
            return await _profileRepository.GetByUserIdAsync(userId);
        }

        public async Task<bool> UpsertProfileAsync(Guid userId, UpsertProfileRequestDto request)
        {
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

            return await _profileRepository.UpsertAsync(profile);
        }
    }
}
