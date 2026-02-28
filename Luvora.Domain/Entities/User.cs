using System;

namespace Luvora.Domain.Entities
{
    public class User
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string Role { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsActive { get; set; }
        private User() // required for Dapper / EF
        {
            Email = null!;
            PasswordHash = null!;
            Role = null!;
        } 
        public User(string email, string passwordHash)
        {
            Id = Guid.NewGuid();
            Email = email;
            PasswordHash = passwordHash;
            Role = "User";
            CreatedAt = DateTime.UtcNow;
            IsActive = true;
        }
    }
}
