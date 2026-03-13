using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luvora.Application.DTOs.Chat
{
    public class SendMessageDto
    {
        public Guid MatchId { get; set; }
        public Guid ReceiverUserId { get; set; }
        public string Content { get; set; } = null!;
    }
}
