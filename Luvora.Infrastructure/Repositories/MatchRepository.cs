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
    public class MatchRepository : IMatchRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public MatchRepository(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }
        public async Task CreateMatchAsync(Match match)
        {
            var sql = @"INSERT INTO Matches
                        (Id, User1Id, User2Id, MatchedAt, IsActive)
                        VALUES
                        (@Id, @User1Id, @User2Id, @MatchedAt, @IsActive)";

            using var connection = _connectionFactory.GetConnection();

            await connection.ExecuteAsync(sql, match);
        }

        public async Task<IEnumerable<Match>> GetMatchesForUserAsync(Guid userId)
        {
            var sql = @"Select * From Matches
                        where User1Id = @UserId or User2Id = @UserId";

            using var connection = _connectionFactory.GetConnection();

            return await connection.QueryAsync<Match>(sql, new { UserId = userId });
        }

        public async Task<bool> MatchExistsAsync(Guid user1Id, Guid user2Id)
        {
            var sql = @"Select Count(1) From Matches
                        Where 
                        (User1Id = @User1Id AND User2Id = @User2Id)
                        OR
                        (User1Id = @User2Id AND User2Id = @User1Id)";

            using var connections = _connectionFactory.GetConnection();

            var count = await connections.ExecuteScalarAsync<int>(sql, new
            {
                User1Id = user1Id,
                User2Id = user2Id
            });

            return count > 0;
        }
    }
}
