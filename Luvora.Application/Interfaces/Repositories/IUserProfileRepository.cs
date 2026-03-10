using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Luvora.Application.DTOs.DiscoveryProfile;
using Luvora.Domain.Entities;
using Luvora.Domain.Enums;

namespace Luvora.Application.Interfaces.Repositories
{
    public interface IUserProfileRepository
    {
        Task<UserProfile?> GetByUserIdAsync(Guid userId);
        Task<bool> UpsertAsync(UserProfile userProfile);
        Task<IEnumerable<DiscoveryProfileDto>> GetDiscoveryProfileAsync(Guid currentUserId, Gender preferredGender, int minAge, int maxAge);
    }
}
