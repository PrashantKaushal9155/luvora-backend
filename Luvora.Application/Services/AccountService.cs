using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Luvora.Application.Interfaces.Repositories;
using Luvora.Application.Interfaces.Services;

namespace Luvora.Application.Services
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepository;

        public AccountService(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public async Task<bool> DeleteAccountAsync(Guid userId)
        {
            var result = await _accountRepository.DeleteAccountAsync(userId);

            return result > 0;
        }
    }
}
