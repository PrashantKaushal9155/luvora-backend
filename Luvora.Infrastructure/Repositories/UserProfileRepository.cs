using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Luvora.Application.DTOs.DiscoveryProfile;
using Luvora.Application.Interfaces.Infrastructure;
using Luvora.Application.Interfaces.Repositories;
using Luvora.Domain.Entities;
using Luvora.Domain.Enums;

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

		public async Task<IEnumerable<DiscoveryProfileDto>> GetDiscoveryProfileAsync(Guid currentUserId, Gender preferredGender, int minAge, int maxAge)
        {
			var sql = @"
			Select top 20
				p.UserId,
				p.Name,
				DATEDIFF(YEAR, p.DateOfBirth, GETUTCDATE()) AS Age,
				p.City,
				p.Bio,
				p.Occupation,
				ph.PhotoUrl As PrimaryPhotoUrl
			FROM UserProfiles p

			Left Join UserPhotos ph
			On ph.UserId = p.UserId
			And ph.IsPrimary = 1

			WHERE p.UserId != @CurrentUserId
			AND p.ProfileCompleted = 1
			AND p.IsDeleted = 0
			AND p.IsVisible = 1
			AND p.Gender = @PreferredGender

			AND DATEDIFF(YEAR, p.DateOfBirth, GETUTCDATE())
				BETWEEN @MinAge AND @MaxAge

			AND NOT EXISTS (
				SELECT 1
				FROM SwipeActions s
				WHERE s.SwiperUserId = @CurrentUserId
				AND s.TargetUserId = p.UserId
			)

			AND NOT EXISTS (
				SELECT 1
				FROM Matches m
				WHERE
				(m.User1Id = @CurrentUserId AND m.User2Id = p.UserId)
				OR
				(m.User1Id = p.UserId AND m.User2Id = @CurrentUserId)
			)";

			using var connection = _connectionFactory.GetConnection();

			return await connection.QueryAsync<DiscoveryProfileDto>(sql, new
			{
				CurrentUserId = currentUserId,
				PreferredGender = preferredGender,
				MinAge = minAge,
				MaxAge = maxAge
			});
		}
    }
}
