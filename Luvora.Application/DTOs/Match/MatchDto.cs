using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luvora.Application.DTOs.Match
{
    public class MatchDto
    {
        public Guid MatchId { get; set; }
        public Guid UserId { get; set; }
        public string Name { get; set; } = null!;
        public int Age { get; set; }
        public string City { get; set; } = null!;
        public string? PhotoUrl { get; set; }
        public DateTime MatchedAt { get; set; }
        public int UnreadMessages { get; set; }
        public string? LastMessage { get; set; }
        public DateTime? LastMessageAt { get; set; }
    }
}
