using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Luvora.Application.Interfaces.Infrastructure;
using Luvora.Application.Interfaces.Repositories;
using Luvora.Domain.Entities;

namespace Luvora.Infrastructure.Repositories
{
    public class SwipeRepository : ISwipeRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public SwipeRepository(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task AddSwipeAsync(SwipeAction swipeAction)
        {
            var sql = @"Insert into SwipeActions
                        (Id, SwiperUserId, TargetUserId, ActionType, CreatedAt)
                        VALUES
                        (@Id, @SwiperUserId, @TargetUserId, @ActionType, @CreatedAt)";

            using var connection = _connectionFactory.GetConnection();

            await connection.ExecuteAsync(sql, swipeAction);
        }

        public async Task<bool> HasUserLikedAsync(Guid swiperUserId, Guid targetUserId)
        {
            var sql = @"SELECT COUNT(1)
                        FROM SwipeActions
                        WHERE SwiperUserId = @SwiperUserId
                        AND TargetUserId = @TargetUserId
                        AND ActionType = 1";

            using var connection = _connectionFactory.GetConnection();

            var count = await connection.ExecuteScalarAsync<int>(sql, new
            { 
                SwiperUserId = swiperUserId,
                TargetUserId = targetUserId
            });

            return count > 0;
        }
    }
}
