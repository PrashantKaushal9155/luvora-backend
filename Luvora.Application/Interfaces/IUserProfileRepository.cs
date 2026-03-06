using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Luvora.Domain.Entities;

namespace Luvora.Application.Interfaces
{
    public interface IUserProfileRepository
    {
        Task<UserProfile?> GetByUserIdAsync(Guid userId);
        Task<bool> UpsertAsync(UserProfile userProfile);
    }
}
