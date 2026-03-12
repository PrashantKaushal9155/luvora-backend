using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Luvora.Application.Interfaces.Infrastructure;
using Luvora.Application.Interfaces.Repositories;

namespace Luvora.Infrastructure.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public AccountRepository(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }
        // Soft delete the account
        public async Task<int> DeleteAccountAsync(Guid userId)
        {
            var sql = @"
            Update Users
            Set IsActive = 0,
            Where Id = @UserId;

            Update UserProfiles
            Set IsDeleted = 1,
                IsVisible = 0
            Where UserId = @UserId;
            ";

            using var connection = _connectionFactory.GetConnection();

            return await connection.ExecuteAsync(sql, new { UserId = userId});
        }
    }
}
