using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Luvora.Application.DTOs.Chat;

namespace Luvora.Application.Interfaces.Services
{
    public interface IChatService
    {
        Task SendMessageAsync(Guid senderId, SendMessageDto message);
        Task<IEnumerable<MessageDto>> GetMessagesAsync(Guid matchId);
        Task MarkMessagesReadAsync(Guid matchId, Guid userId);
    }
}
