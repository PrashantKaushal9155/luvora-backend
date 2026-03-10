using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Luvora.Application.DTOs.Swipe;

namespace Luvora.Application.Interfaces.Services
{
    public interface ISwipeService
    {
        Task<SwipeResponseDto> SwipeAsync(Guid swiperUserId, SwipeRequestDto swipeRequest);
    }
}
