using Luvora.Application.DTOs.Chat;
using Luvora.Application.Interfaces.Services;
using Luvora.Infrastructure.RealTime;
using Microsoft.AspNetCore.SignalR;

namespace Luvora.API.Hubs
{
    public class ChatHub : Hub
    {
        private readonly IChatService _chatService;
        private readonly PresenceTracker _presenceTracker;

        public ChatHub(IChatService chatService, PresenceTracker presenceTracker)
        {
            _chatService = chatService;
            _presenceTracker = presenceTracker;
        }

        public override async Task OnConnectedAsync()
        {
            var userId = Context.UserIdentifier;

            await _presenceTracker.UserConnected(userId!, Context.ConnectionId);

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var userId = Context.UserIdentifier;

            await _presenceTracker.UserDisconnected(userId!, Context.ConnectionId);

            await base.OnDisconnectedAsync(exception);
        }

        public async Task SendMessage(SendMessageDto messageDto)
        {
            var senderId = Guid.Parse(Context.UserIdentifier!);

            await _chatService.SendMessageAsync(senderId, messageDto);

            await Clients.Group(messageDto.MatchId.ToString())
                .SendAsync("ReceiveMessage", messageDto);
        }

        public async Task Typing(Guid matchId)
        {
            await Clients.OthersInGroup(matchId.ToString())
                .SendAsync("UserTyping");
        }

        public async Task JoinMatchRoom(Guid matchId)
        {
            var groupName = matchId.ToString();

            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
        }

        public async Task LeaveMatchRoom(Guid matchId)
        {
            var groupName = matchId.ToString();

            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
        }
    }
}
