using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Luvora.Application.DTOs.Match;
using Luvora.Domain.Entities;

namespace Luvora.Application.Interfaces.Repositories
{
    public interface IMatchRepository
    {
        Task CreateMatchAsync(Match match);
        Task <bool> MatchExistsAsync(Guid user1Id, Guid user2Id);
        Task<IEnumerable<Match>> GetMatchesForUserAsync(Guid userId);
        Task<IEnumerable<MatchDto>> GetUserMatchesAsync(Guid currentUserId);
    }
}
