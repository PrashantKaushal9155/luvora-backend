using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Luvora.Domain.Entities;

namespace Luvora.Application.Interfaces.Repositories
{
    public interface ISwipeRepository
    {
        Task AddSwipeAsync(SwipeAction swipeAction);
        Task<bool> HasUserLikedAsync(Guid swiperUserId, Guid targetUserId);
    }
}
