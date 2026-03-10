using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luvora.Application.DTOs.Swipe
{
    public class SwipeResponseDto
    {
        public bool IsMatch { get; set; }
        public Guid? MatchUserId { get; set; }
    }
}
