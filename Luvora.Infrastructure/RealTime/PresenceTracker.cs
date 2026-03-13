using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luvora.Infrastructure.RealTime
{
    public class PresenceTracker
    {
        private static readonly Dictionary<string, List<string>> OnlineUsers = new();

        public Task UserConnected(string userId, string connectionId)
        {
            lock (OnlineUsers)
            {
                if(!OnlineUsers.ContainsKey(userId))
                    OnlineUsers[userId] = new List<string>();

                OnlineUsers[userId].Add(connectionId);
            }

            return Task.CompletedTask;
        }

        public Task UserDisconnected(string userId, string connectionId)
        {
            lock(OnlineUsers)
            {
                if(!OnlineUsers.ContainsKey(userId))
                    return Task.CompletedTask;

                OnlineUsers[userId].Remove(connectionId);

                if (OnlineUsers[userId].Count == 0)
                    OnlineUsers.Remove(userId);
            }

            return Task.CompletedTask;
        }

        public bool IsOnline(string userId)
        {
            return OnlineUsers.ContainsKey(userId);
        }
    }
}
