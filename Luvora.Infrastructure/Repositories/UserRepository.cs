using Dapper;
using Luvora.Application.Interfaces;
using Luvora.Domain.Entities;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Data;
using System.Threading.Tasks;

namespace Luvora.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;
        public UserRepository(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task AddAsync(User user)
        {
            var sql = @"
                Insert into Users (Id, Email, PasswordHash, Role, CreatedAt, IsActive)
                Values (@Id, @Email, @PasswordHash, @Role, @CreatedAt, @IsActive)";

            using var connection = _connectionFactory.GetConnection();
            await connection.ExecuteAsync(sql, user);

        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            var sql = "SELECT * FROM Users WHERE Email = @Email";

            using var connection = _connectionFactory.GetConnection();
            return await connection.QueryFirstOrDefaultAsync<User>(sql, new { Email = email });
        }

        public async Task<User?> GetByIdAsync(Guid id)
        {
            var sql = @"SELECT * FROM Users WHERE Id = @Id";

            using var connection = _connectionFactory.GetConnection();
            return await connection.QueryFirstOrDefaultAsync<User>(sql, new {Id = id});
        }

        public async Task<bool> UpdatePasswordAsync(Guid userId, string hashedPassword)
        {
            var sql = @"UPDATE Users SET PasswordHash = @PasswordHash WHERE Id = @UserId";

            using var connection = _connectionFactory.GetConnection();
            var result = await connection.ExecuteAsync(sql, new
            {
                PasswordHash = hashedPassword,
                UserId = userId
            });

            return result > 0;
        }
    }
}
