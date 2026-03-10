using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Luvora.Application.DTOs.Swipe;
using Luvora.Application.Interfaces.Repositories;
using Luvora.Application.Interfaces.Services;
using Luvora.Domain.Entities;
using Luvora.Domain.Enums;

namespace Luvora.Application.Services
{
    // Heart of the application (Matching Engine)
    public class SwipeService : ISwipeService
    {
        private readonly ISwipeRepository _swipeRepository;
        private readonly IMatchRepository _matchRepository;

        public SwipeService(ISwipeRepository swipeRepository, IMatchRepository matchRepository)
        {
            _swipeRepository = swipeRepository;
            _matchRepository = matchRepository;
        }

        public async Task<SwipeResponseDto> SwipeAsync(Guid swiperUserId, SwipeRequestDto request)
        {
            var swipe = new SwipeAction
            (
                swiperUserId,
                request.TargetUserId,
                request.ActionType
            );

            await _swipeRepository.AddSwipeAsync( swipe );

            // Only Like can create a match
            if (request.ActionType != SwipeActionType.Like)
            {
                return new SwipeResponseDto
                {
                    IsMatch = false
                };
            }

            // Check reverse like
            var reverseLike = await _swipeRepository.HasUserLikedAsync(
                request.TargetUserId,
                swiperUserId
            );

            if(!reverseLike)
            {
                return new SwipeResponseDto
                {
                    IsMatch = false
                };
            }

            // Check if match already exists
            var matchExists = await _matchRepository.MatchExistsAsync(
                swiperUserId,
                request.TargetUserId
            );

            if(matchExists)
            {
                return new SwipeResponseDto
                {
                    IsMatch = false
                };
            }

            // Create a new Match
            var match = new Match
            (
                swiperUserId,
                request.TargetUserId
            );

            await _matchRepository.CreateMatchAsync( match );

            return new SwipeResponseDto
            {
                IsMatch = true,
                MatchUserId = request.TargetUserId
            };
        }
    }
}
