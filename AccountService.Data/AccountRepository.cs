using Account.Data.Entites;
using Account.Share.Interfaces;
using Account.Share.Models;
using AutoMapper;
using Exceptions;
using Messages.Commands;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace Account.Data
{
    public class AccountRepository : IAccountRepository
    {
        private readonly AccountContext _accountContext;
        private readonly IMapper _mapper;
        public AccountRepository(AccountContext accountContext, IMapper mapper)
        {
            _accountContext = accountContext;
            _mapper = mapper;
        }

        public async Task<bool> CheckAccountCorrectness(Guid accountId)
        {
            AccountEntity account = await _accountContext.Accounts
            .FirstOrDefaultAsync(account => account.Id == accountId);
            if (account == null)
            {
                return false;
            }
            return true;
        }

        public async Task<int> CheckBalance(Guid fromAccountId, int amount)
        {
            AccountEntity account = await _accountContext.Accounts
            .FirstOrDefaultAsync(account => account.Id == fromAccountId);
            return account.Balance;
        }

        public async Task<AccountsBalance> UpdateAccounts(UpdateAccounts message)
        {
            AccountEntity fromAccount = await _accountContext.Accounts
            .FirstOrDefaultAsync(account => account.Id == message.FromAccountId);

            fromAccount.Balance -= message.Amount;
            AccountEntity toAccount = await _accountContext.Accounts
                .FirstOrDefaultAsync(account => account.Id == message.ToAccountId);
            toAccount.Balance += message.Amount;

            await _accountContext.SaveChangesAsync();
            AccountsBalance accountsBalance = new AccountsBalance()
            {
                BalanceOfFromAccount = fromAccount.Balance,
                BalanceOfToAccount = toAccount.Balance
            };
            return accountsBalance;
        }

        public async Task<AccountModel> GetAccountInfoAsync(Guid accountId)
        {
            AccountEntity accountEntity = await _accountContext.Accounts
                .Include(c => c.Customer)
                .FirstOrDefaultAsync(a => a.Id == accountId);
            if (accountEntity != null)
            {
                AccountModel accountModel = _mapper.Map<AccountModel>(accountEntity);
                return accountModel;
            }
            else
            {
                throw new AccountNotFoundException();
            }
        }
    }
}
