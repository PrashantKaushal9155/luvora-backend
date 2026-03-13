using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Luvora.Application.DTOs.Chat;
using Luvora.Application.Interfaces.Infrastructure;
using Luvora.Application.Interfaces.Repositories;
using Luvora.Domain.Entities;

namespace Luvora.Infrastructure.Repositories
{
    public class ChatRepository : IChatRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public ChatRepository(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task SaveMessageAsync(Message message)
        {
            var sql = @"
            Insert Into Messages
            (Id, MatchId, SenderUserId, ReceiverUserId, Content, IsRead, CreatedAt)
            Values
            (@Id, @MatchId, @SenderUserId, @ReceiverUserId, @Content, @IsRead, @CreatedAt)";

            using var connection = _connectionFactory.GetConnection();

            await connection.ExecuteAsync(sql, message);
        }

        public async Task<IEnumerable<MessageDto>> GetMessageAsync(Guid matchId)
        {
            var sql = @"Select
                        Id, SenderUserId, Content, IsRead, CreatedAt
                        From Messages
                        Where MatchId = @MatchId
                        Order By CreatedAt";

            using var connection = _connectionFactory.GetConnection();

            return await connection.QueryAsync<MessageDto>(sql, new { MatchId = matchId });
        }

        public async Task MarkMessagesAsReadAsync(Guid matchId, Guid userId)
        {
            var sql = @"Update Messages
                        Set IsRead = 1
                        Where MatchId = @MatchId
                        And ReceiverUserId = @UserId
                        And IsRead = 0";

            using var connection = _connectionFactory.GetConnection();

            await connection.ExecuteAsync(sql, new
            {
                MatchId = matchId,
                UserId = userId
            });
        }
    }
}
