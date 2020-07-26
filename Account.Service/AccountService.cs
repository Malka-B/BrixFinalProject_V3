using Account.Service.Intefaces;
using Account.Service.Models;
using Account.Share.Interfaces;
using Account.Share.Models;
using System;
using System.Threading.Tasks;


namespace Account.Service
{
    public class AccountService : IAccountService
    {
        IAccountRepository _accountRepository;
        public AccountService(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public async Task<AccountModel> GetAccountInfoAsync(Guid CustomerId)
        {
            return await _accountRepository.GetAccountInfoAsync(CustomerId);
        }

        
    }
}
