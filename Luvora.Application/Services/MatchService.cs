using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Luvora.Application.DTOs.Match;
using Luvora.Application.Interfaces.Repositories;
using Luvora.Application.Interfaces.Services;

namespace Luvora.Application.Services
{
    public class MatchService : IMatchService
    {
        private readonly IMatchRepository _matchRepository;

        public MatchService(IMatchRepository matchRepository)
        {
            _matchRepository = matchRepository;
        }

        public async Task<IEnumerable<MatchDto>> GetUserMatchesAsync(Guid userId)
        {
            return await _matchRepository.GetUserMatchesAsync(userId);
        }
    }
}
