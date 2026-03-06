using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Luvora.Application.Interfaces;
using Luvora.Domain.Entities;

namespace Luvora.Infrastructure.Repositories
{
    public class UserProfileRepository : IUserProfileRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public UserProfileRepository(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }
        public async Task<UserProfile?> GetByUserIdAsync(Guid userId)
        {
            var sql = @"Select * From UserProfiles where UserId = @UserId";
            using var connection = _connectionFactory.GetConnection();
            return await connection.QueryFirstOrDefaultAsync<UserProfile>(sql, new {UserId = userId});
        }

        public async Task<bool> UpsertAsync(UserProfile userProfile)
        {
			var sql = @"If Exists (Select 1 from UserProfiles where UserId = @UserId)
						Begin
							 Update UserProfiles
							 Set
								City = @City,
								Bio = @Bio,
								Gender = @Gender,
								PreferredGender = @PreferredGender,
								DateOfBirth = @DateOfBirth,
								MinPreferredAge = @MinPreferredAge,
								MaxPreferredAge = @MaxPreferredAge,
								Occupation = @Occupation,
								RelationshipStatus = @RelationshipStatus,
								ProfileCompleted = @ProfileCompleted
							Where UserId = @UserId
						End
						Else
						Begin
							Insert Into UserProfiles
							(
								Id,
								UserId,
								City,
								Bio,
								Gender,
								PreferredGender,
								DateOfBirth,
								MinPreferredAge,
								MaxPreferredAge,
								Occupation,
								RelationshipStatus,
								ProfileCompleted,
								CreatedAt
							)
							Values
							(
								@Id,
								@UserId,
								@City,
								@Bio,
								@Gender,
								@PreferredGender,
								@DateOfBirth,
								@MinPreferredAge,
								@MaxPreferredAge,
								@Occupation,
								@RelationshipStatus,
								@ProfileCompleted,
								@CreatedAt
							)
						End";

			using var connection = _connectionFactory.GetConnection();

			var result = await connection.ExecuteAsync(sql, userProfile);

			return result > 0;
        }
    }
}
