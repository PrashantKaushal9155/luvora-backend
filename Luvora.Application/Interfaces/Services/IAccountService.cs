using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luvora.Application.Interfaces.Services
{
    public interface IAccountService
    {
        Task<bool> DeleteAccountAsync(Guid userId);
    }
}
