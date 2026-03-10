using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Luvora.Domain.Enums;

namespace Luvora.Domain.Entities
{
    public class SwipeAction
    {
        public Guid Id { get; set; }
        public Guid SwiperUserId { get; set; }
        public Guid TargetUserId { get; set; }

        public SwipeActionType ActionType { get; set; }

        public DateTime CreatedAt { get; set; }

        private SwipeAction() { }

        public SwipeAction(Guid swiperUserId, Guid targetUserId, SwipeActionType actionType)
        {
            Id = Guid.NewGuid();
            SwiperUserId = swiperUserId;
            TargetUserId = targetUserId;
            ActionType = actionType;
            CreatedAt = DateTime.UtcNow;
        }
    }
}
