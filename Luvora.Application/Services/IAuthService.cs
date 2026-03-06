using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Luvora.Application.DTOs.Auth;

namespace Luvora.Application.Services
{
    public interface IAuthService
    {
        Task RegisterAsync(string email, string password);
        Task<LoginResponseDto> LoginAsync(string email, string password);
        Task<(string accessToken, string refreshToken)> RefreshAsync(string refreshToken);
        Task ChangePasswordAsync(Guid userId, ChangePasswordRequestDto request);
    }
}
