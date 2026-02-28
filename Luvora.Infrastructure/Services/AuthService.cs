using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Luvora.Application.Common;
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
        public AuthService(IUserRepository userRepository, IOptions<JwtSettings> jwtOptions)
        {
            _userRepository = userRepository;
            _jwtSettings = jwtOptions.Value
                ?? throw new ArgumentNullException(nameof(jwtOptions));
        }

        public async Task RegisterAsync(string email, string password)
        {
            var existingUser = await _userRepository.GetByEmailAsync(email);
            Console.WriteLine(existingUser == null ? "NULL" : "NOT NULL");
            if (existingUser != null)
                throw new Exception("User already exists.");

            var passwordHash = BCrypt.Net.BCrypt.HashPassword(password);
            var user = new User(email, passwordHash);
            await _userRepository.AddAsync(user);
        }

        public async Task<string> LoginAsync(string email, string password)
        {
            var user = await _userRepository.GetByEmailAsync(email) ?? throw new Exception("Invalid User Credentials.");

            var isPasswordValid = BCrypt.Net.BCrypt.Verify(password, user.PasswordHash);

            if (!isPasswordValid)
                throw new Exception("Invalid password.");

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
    }
}
