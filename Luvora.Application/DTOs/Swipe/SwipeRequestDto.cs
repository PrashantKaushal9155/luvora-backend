using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Luvora.Domain.Enums;

namespace Luvora.Application.DTOs.Swipe
{
    public class SwipeRequestDto
    {
        public Guid TargetUserId { get; set; }
        public SwipeActionType ActionType { get; set; }
    }
}
