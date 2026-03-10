using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Luvora.Application.DTOs.Profile;
using Luvora.Domain.Entities;

namespace Luvora.Application.Interfaces.Services
{
    public interface IProfileService
    {
        Task<bool> UpsertProfileAsync(Guid userId, UpsertProfileRequestDto request);
        Task<UserProfile?> GetProfileAsync(Guid userId);
    }
}
