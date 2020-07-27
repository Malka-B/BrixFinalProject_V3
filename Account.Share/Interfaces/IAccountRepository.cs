
using Account.Share.Models;
using Messages.Commands;
using System;
using System.Threading.Tasks;

namespace Account.Share.Interfaces
{
    public interface IAccountRepository
    {
        Task<AccountModel> GetAccountInfoAsync(Guid CustomerId);
        Task<bool> CheckAccountCorrectness(Guid accountId);
        Task<bool> CheckBalance(Guid fromAccountId, int amount);
        Task<bool> UpdateAccounts(UpdateAccounts message);
    }
}
