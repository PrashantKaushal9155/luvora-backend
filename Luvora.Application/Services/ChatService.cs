using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Luvora.Application.DTOs.Chat;
using Luvora.Application.Interfaces.Repositories;
using Luvora.Application.Interfaces.Services;
using Luvora.Domain.Entities;

namespace Luvora.Application.Services
{
    public class ChatService : IChatService
    {
        private readonly IChatRepository _chatRepository;

        public ChatService(IChatRepository chatRepository)
        {
            _chatRepository = chatRepository;
        }

        public async Task SendMessageAsync(Guid senderId, SendMessageDto messageDto)
        {
            var message = new Message(
                messageDto.MatchId,
                senderId,
                messageDto.ReceiverUserId,
                messageDto.Content
            );

            await _chatRepository.SaveMessageAsync(message);
        }

        public async Task<IEnumerable<MessageDto>> GetMessagesAsync(Guid matchId)
        {
            return await _chatRepository.GetMessageAsync(matchId);
        }

        public async Task MarkMessagesReadAsync(Guid matchId, Guid userId)
        {
            await _chatRepository.MarkMessagesAsReadAsync(matchId, userId);
        }
    }
}
