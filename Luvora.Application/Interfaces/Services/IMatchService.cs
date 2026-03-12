using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Luvora.Application.DTOs.Match;

namespace Luvora.Application.Interfaces.Services
{
    public interface IMatchService
    {
        Task<IEnumerable<MatchDto>> GetUserMatchesAsync(Guid userId);
    }
}
