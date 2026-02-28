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
        private readonly string _connectionString;
        public UserRepository(IConfiguration configuration)
        {
            _connectionString = configuration
                .GetConnectionString("DefaultConnection")
                ?? throw new ArgumentNullException("DefaultConnection not found");
        }

        private IDbConnection CreateConnection()
            => new SqlConnection(_connectionString);

        public async Task AddAsync(User user)
        {
            const string sql = @"
                Insert into Users (Id, Email, PasswordHash, Role, CreatedAt, IsActive)
                Values (@Id, @Email, @PasswordHash, @Role, @CreatedAt, @IsActive)";

            using var connection = CreateConnection();
            await connection.ExecuteAsync(sql, user);

        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);

                var sql = "SELECT * FROM Users WHERE Email = @Email";

                return await connection.QueryFirstOrDefaultAsync<User>(sql, new { Email = email });
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR: " + ex.Message);
                throw;
            }
        }

        public async Task<User?> GetByIdAsync(Guid id)
        {
            const string sql = @"
                Select * from Users
                Where Id = @Id";
            using var connection = CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<User>(sql, new {Id = id});
        }
    }
}
