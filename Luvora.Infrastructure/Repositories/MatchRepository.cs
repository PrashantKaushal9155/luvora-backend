using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Luvora.Application.DTOs.Match;
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

        public async Task<IEnumerable<MatchDto>> GetUserMatchesAsync(Guid currentUserId)
        {
            var sql = @"
            Select
                m.Id As MatchId,
                p.UserId,
                p.Name,
                DATEDIFF(YEAR, p.DateOfBirth, GETUTCDATE()) AS Age,
                p.City,
                ph.PhotoUrl,
                m.CreatedAt As MatchedAt,
                ISNULL(u.UnreadCount,0) AS UnreadMessages,
                lm.Content As LastMessage,
                lm.CreatedAt As LastMessageAt
            From Matches m
            Join UserProfiles p
            On p.UserId =
            Case
                When m.User1Id = @CurrentUserId Then m.User2Id
                Else m.UserId
            End

            Left Join UserPhotos ph
            On ph.UserId = p.UserId
            And ph.IsPrimary = 1

            Left Join
            (
                Selelct MatchId, Count(*) As UnreadCount
                From Messages
                Where ReceiverUserId = @UserId
                And IsRead = 0
                Group By MatchId
            ) u
            On u.MatchId = m.Id

            Outer Apply
            (
                Select Top 1 Content, CreatedAt
                From Messages
                Where MatchId = m.Id
                Order By CreatedAt Desc
            )

            Where
            m.User1Id = @CurrentUserId
            Or m.User2Id = @CurrentUserId

            Order By m.CreatedAt Desc
            ";

            using var connection = _connectionFactory.GetConnection();

            return await connection.QueryAsync<MatchDto>(sql, new
            {
                CurrentUserId = currentUserId
            });
        }
    }
}
