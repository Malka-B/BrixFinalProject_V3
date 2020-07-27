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

        public async Task<bool> CheckAccountsCorrectness(Guid fromAccountId, Guid toAccountId)
        {
            try
            {
                var fromAccount = await _accountContext.Accounts
                .FirstOrDefaultAsync(account => account.CustomerId == fromAccountId);
                var toAccount = await _accountContext.Accounts
                    .FirstOrDefaultAsync(account => account.CustomerId == toAccountId);
                if (fromAccount == null || toAccount == null)
                {
                    return false;
                }
                return true;
            }
            catch
            {
                throw new SystemException();
            }
        }

        public async Task<bool> CheckBalance(Guid fromAccountId, int amount)
        {
            try
            {
                var account = await _accountContext.Accounts
                .FirstOrDefaultAsync(account => account.CustomerId == fromAccountId);
                if (account.Balance < amount)
                {
                    return false;
                }
                return true;
            }
            catch
            {
                throw new SystemException();
            }
        }

        public async Task<bool> UpdateAccounts(UpdateAccounts message)
        {
            try
            {
                var fromAccount = await _accountContext.Accounts
                .FirstOrDefaultAsync(account => account.CustomerId == message.FromAccountId);
                fromAccount.Balance -= message.Amount;
                var toAccount = await _accountContext.Accounts
                    .FirstOrDefaultAsync(account => account.CustomerId == message.ToAccountId);
                toAccount.Balance += message.Amount;
                await _accountContext.SaveChangesAsync();
                return true;
            }
            catch
            {
                throw new SystemException();
            }
        }

        public async Task<AccountModel> GetAccountInfoAsync(Guid CustomerId)
        {
            try
            {
                AccountEntity accountEntity = await _accountContext.Accounts
                    .Include(c => c.Customer)
                    .FirstOrDefaultAsync(a => a.CustomerId == CustomerId);
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
            catch (Exception e)
            {
                if (e is AccountNotFoundException)
                {
                    throw new AccountNotFoundException();
                }
                else
                {
                    throw new SystemException();
                }
            }
        }
    }
}

