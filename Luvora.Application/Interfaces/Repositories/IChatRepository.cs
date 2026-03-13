using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Luvora.Application.DTOs.Chat;
using Luvora.Domain.Entities;

namespace Luvora.Application.Interfaces.Repositories
{
    public interface IChatRepository
    {
        Task SaveMessageAsync(Message message);
        Task<IEnumerable<MessageDto>> GetMessageAsync(Guid matchId);
        Task MarkMessagesAsReadAsync(Guid matchId, Guid userId);
    }
}
