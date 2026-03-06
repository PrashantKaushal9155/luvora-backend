using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luvora.Application.DTOs.Auth
{
    public class LoginResponseDto
    {
        public string AccessToken { get; set; } = null!;
        public string RefreshToken { get; set; } = null!;
        public bool ProfileCompleted { get; set; }
    }
}
