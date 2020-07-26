using Account.Share.Interfaces;
using Messages.Commands;
using Messages.Events;
using NServiceBus;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Account.NSB
{
    public class AccountHendler : IHandleMessages<UpdateAccounts>
    {
        private readonly IAccountRepository _accountRepository;

        public AccountHendler(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }
        public async Task Handle(UpdateAccounts message, IMessageHandlerContext context)
        {
            AccountsUpdated accountsUpdated = new AccountsUpdated
            {
                TransactionId = message.TransactionId
            };
            //bool areAccountsExist = await _accountRepository
            //    .CheckAccountsCorrectness(message.FromAccountId, message.ToAccountId);
            //if (areAccountsExist == false )
            //{
            //    return false;
            //}
            //bool isBalanceEnough = await _accountRepository
            //    .CheckBalance(message.FromAccountId, message.Amount);
            //if (isBalanceEnough == false)
            //{
            //    return false;
            //}           

            //bool isAccountsUpdated = await _accountRepository.UpdateAccounts(message);
            bool isAccountsUpdated = true;
            if (isAccountsUpdated == false )
            {
                accountsUpdated.isAccountsUpdateSuccess = true;
            }
            else
            {
                accountsUpdated.isAccountsUpdateSuccess = false;
            }
            await context.Publish(accountsUpdated)
                .ConfigureAwait(false);
        }
    }
}


