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
    public class UserPhotoRepository : IUserPhotoRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public UserPhotoRepository(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task AddPhotoAsync(UserPhoto photo)
        {
            var sql = @"
            Insert into UserPhotos
            (Id, UserId, PhotoUrl, DisplayOrder, IsPrimary, CreatedAt)
            Values
            (@Id, @UserId, @PhotoUrl, @DisplayOrder, @IsPrimary, @CreatedAt)";

            using var connection = _connectionFactory.GetConnection();

            await connection.ExecuteAsync(sql, photo);
        }

        public async Task<IEnumerable<UserPhoto>> GetPhotosByUserIdAsync(Guid userId)
        {
            var sql = @"Select * from UserPhotos Where UserId = @UserId Order By DisplayOrder";

            var connection = _connectionFactory.GetConnection();

            return await connection.QueryAsync<UserPhoto>(sql, new { UserId = userId});
        }

        public async Task DeletePhotoAsync(Guid photoId, Guid userId)
        {
            var sql = @"Delete From UserPhoto where Id = @PhotoId and UserId = @UserId";

            using var connection = _connectionFactory.GetConnection();

            await connection.ExecuteAsync(sql, new { PhotoId = photoId, UserId = userId });
        }

        public async Task<int> GetPhotoCountAsync(Guid userId)
        {
            var sql = @"Select Count(*) From UserPhotos Where UserId = @UserId";

            using var connection = _connectionFactory.GetConnection();

            return await connection.ExecuteScalarAsync<int>(sql, new { UserId = userId});
        }
    }
}
