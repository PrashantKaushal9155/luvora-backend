using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luvora.Domain.Entities
{
    public class Message
    {
        public Guid Id { get; set; }
        public Guid MatchId { get; set; }
        public Guid SenderUserId { get; set; }
        public Guid ReceiverUserId { get; set; }
        public string Content { get; set; } = null!;
        public bool IsRead { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsDeleted { get; set; }
        private Message() { }
        public Message(Guid matchId, Guid senderId, Guid receiverUserId, string content)
        {
            Id = Guid.NewGuid();
            MatchId = matchId;
            SenderUserId = senderId;
            ReceiverUserId = receiverUserId;
            Content = content;

            IsRead = false;
            CreatedAt = DateTime.UtcNow;
            IsDeleted = false;
        }
    }
}
