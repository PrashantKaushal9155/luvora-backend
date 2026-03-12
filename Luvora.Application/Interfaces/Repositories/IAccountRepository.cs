using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luvora.Application.Interfaces.Repositories
{
    public interface IAccountRepository
    {
        Task<int> DeleteAccountAsync(Guid userId);
    }
}
