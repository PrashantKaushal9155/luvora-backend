using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Luvora.Application.DTOs.DiscoveryProfile;

namespace Luvora.Application.Interfaces.Services
{
    public interface IDiscoveryProfileService
    {
        Task<IEnumerable<DiscoveryProfileDto>> GetDiscoveryProfilesAsync(Guid userId);
    }
}
