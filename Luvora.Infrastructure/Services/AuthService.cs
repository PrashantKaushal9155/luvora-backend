using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Luvora.Application.Common;
using Luvora.Application.DTOs.Auth;
using Luvora.Application.Interfaces;
using Luvora.Application.Services;
using Luvora.Domain.Entities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Luvora.Infrastructure.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly JwtSettings _jwtSettings;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IUserProfileRepository _userProfileRepository;

        public AuthService(
            IUserRepository userRepository, 
            IOptions<JwtSettings> jwtOptions, 
            IRefreshTokenRepository refreshTokenRepository,
            IUserProfileRepository userProfileRepository)
        {
            _userRepository = userRepository;
            _jwtSettings = jwtOptions.Value
                ?? throw new ArgumentNullException(nameof(jwtOptions));
            _refreshTokenRepository = refreshTokenRepository;
            _userProfileRepository = userProfileRepository;
        }

        public async Task RegisterAsync(string email, string password)
        {
            var existingUser = await _userRepository.GetByEmailAsync(email);
            if (existingUser != null)
                throw new Exception("User already exists.");

            var passwordHash = BCrypt.Net.BCrypt.HashPassword(password);
            var user = new User(email, passwordHash);
            await _userRepository.AddAsync(user);
        }

        public async Task<LoginResponseDto> LoginAsync(string email, string password)
        {
            var user = await _userRepository.GetByEmailAsync(email) ?? throw new Exception("Invalid User Credentials.");

            var isPasswordValid = BCrypt.Net.BCrypt.Verify(password, user.PasswordHash);

            if (!isPasswordValid)
                throw new Exception("Invalid password.");

            var accessToken = GenerateJwtToken(user);
            var refreshedTokenValue = Guid.NewGuid().ToString();
            var refreshedToken = new RefreshToken(
                user.Id,
                refreshedTokenValue,
                DateTime.UtcNow.AddDays(7)
                );

            await _refreshTokenRepository.AddAsync(refreshedToken);

            var profile = await _userProfileRepository.GetByUserIdAsync(user.Id);
            Console.WriteLine("UserProfile:", profile);
            return new LoginResponseDto
            {
                AccessToken = accessToken,
                RefreshToken = refreshedTokenValue,
                ProfileCompleted = profile != null && profile.ProfileCompleted
            };
        }

        private string GenerateJwtToken(User user)
        {

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiryMinutes),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<(string accessToken, string refreshToken)> RefreshAsync(string refreshToken)
        {
            var storedToken = await _refreshTokenRepository.GetByTokenAsync(refreshToken);

            if (storedToken == null ||
                storedToken.IsRevoked ||
                storedToken.ExpiryDate <= DateTime.UtcNow)
                throw new Exception("Invalid refresh token");

            var user = await _userRepository.GetByIdAsync(storedToken.UserId);

            if (user == null)
                throw new Exception("User not found");

            await _refreshTokenRepository.RevokeAsync(storedToken.Id);

            var newAccessToken = GenerateJwtToken(user);

            var newRefreshTokenValue = Guid.NewGuid().ToString();
            var newRefreshToken = new RefreshToken(
                user.Id,
                newRefreshTokenValue,
                DateTime.UtcNow.AddDays(7)
                );

            await _refreshTokenRepository.AddAsync(newRefreshToken);

            return (newAccessToken, newRefreshTokenValue);
        }

        public async Task ChangePasswordAsync(Guid userId, ChangePasswordRequestDto request)
        {
            var user = await _userRepository.GetByIdAsync(userId);

            if (user == null)
                throw new Exception("User not found");

            if (!BCrypt.Net.BCrypt.Verify(request.CurrentPassword, user.PasswordHash))
                throw new Exception("Current password is incorrect");

            if (BCrypt.Net.BCrypt.Verify(request.NewPassword, user.PasswordHash))
                throw new Exception("New password cannot be same as current password");

            var newHashedPassword = BCrypt.Net.BCrypt.HashPassword(request.NewPassword);

            var updated = await _userRepository.UpdatePasswordAsync(userId, newHashedPassword);
            await _refreshTokenRepository.RevokeAllByUserIdAsync(userId);

            if (!updated)
                throw new Exception("Password update failed");
        }
    }
}
