using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Luvora.Application.Interfaces.Infrastructure;
using Luvora.Application.Interfaces.Repositories;
using Luvora.Domain.Entities;
using Microsoft.Extensions.Configuration;

namespace Luvora.Infrastructure.Repositories
{
    public class RefreshTokenRepository : IRefreshTokenRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public RefreshTokenRepository(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }
        public async Task AddAsync(RefreshToken token)
        {
            const string sql = @"
                INSERT INTO RefreshTokens 
                (Id, UserId, Token, ExpiryDate, IsRevoked, CreatedAt)
                VALUES
                (@Id, @UserId, @Token, @ExpiryDate, @IsRevoked, @CreatedAt)";

            using var connection = _connectionFactory.GetConnection();
            await connection.ExecuteAsync(sql, token);
        }

        public async Task<RefreshToken?> GetByTokenAsync(string token)
        {
            const string sql = @"
                SELECT * FROM RefreshTokens
                WHERE Token = @Token";

            using var connection = _connectionFactory.GetConnection();
            return await connection.QueryFirstOrDefaultAsync<RefreshToken>(sql, new { Token = token });
        }

        public async Task RevokeAsync(Guid id)
        {
            const string sql = @"
                UPDATE RefreshTokens
                SET IsRevoked = 1
                WHERE Id = @Id";

            using var connection = _connectionFactory.GetConnection();
            await connection.ExecuteAsync(sql, new { Id = id });
        }

        public async Task RevokeAllByUserIdAsync(Guid userId)
        {
            const string sql = @"
                UPDATE RefreshTokens
                SET IsRevoked = 1
                WHERE UserId = @UserId
                AND IsRevoked = 0";

            using var connection = _connectionFactory.GetConnection();
            await connection.ExecuteAsync(sql, new { UserId = userId });
        }
    }
}
