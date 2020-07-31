using Account.Share.Models;
using Messages.Commands;
using System;
using System.Threading.Tasks;

namespace Account.Share.Interfaces
{
    public interface IAccountRepository
    {
        Task<AccountModel> GetAccountInfoAsync(Guid accountId);
        Task<bool> CheckAccountCorrectness(Guid accountId);
        Task<int> CheckBalance(Guid fromAccountId, int amount);
        Task<AccountsBalance> UpdateAccounts(UpdateAccounts message);
    }
}
