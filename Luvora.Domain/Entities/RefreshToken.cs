using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luvora.Domain.Entities
{
    public class RefreshToken
    {
        public Guid Id { get; private set; }
        public Guid UserId { get; private set; }
        public string Token { get; private set; }
        public DateTime ExpiryDate { get; private set; }
        public bool IsRevoked { get; private set; }
        public DateTime CreatedAt { get; private set; }

        private RefreshToken() 
        {
            Token = null!;
        }

        public RefreshToken(Guid userId, string token, DateTime expiry)
        {
            Id = Guid.NewGuid();
            UserId = userId;
            Token = token;
            ExpiryDate = expiry;
            CreatedAt = DateTime.UtcNow;
            IsRevoked = false;
        }
    }
}
