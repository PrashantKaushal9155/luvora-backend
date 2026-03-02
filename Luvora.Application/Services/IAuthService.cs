using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luvora.Application.Services
{
    public interface IAuthService
    {
        Task RegisterAsync(string email, string password);
        Task<(string accessToken, string refreshToken)> LoginAsync(string email, string password);
        Task<(string accessToken, string refreshToken)> RefreshAsync(string refreshToken);
    }
}
