using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luvora.Domain.Entities
{
    public class Match
    {
        public Guid Id { get; set; }

        public Guid User1Id { get; set; }
        public Guid User2Id { get; set; }

        public DateTime MatchedAt { get; set; }
        public bool IsActive { get; set; }

        private Match() { }

        public Match(Guid user1Id, Guid user2Id)
        {
            Id = Guid.NewGuid();
            User1Id = user1Id;
            User2Id = user2Id;
            MatchedAt = DateTime.UtcNow;
            IsActive = true;
        }
    }
}
