using Account.Share.Interfaces;
using Messages.Commands;
using Messages.Events;
using NServiceBus;
using System;
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

            bool isFromAccountExist = await _accountRepository
                .CheckAccountCorrectness(message.FromAccountId);

            bool isToAccountExist = await _accountRepository
                .CheckAccountCorrectness(message.ToAccountId);

            if (isFromAccountExist == false || isToAccountExist == false)
            {
                accountsUpdated.FailureReason = "One of the accounts Id is not coorrect";
                accountsUpdated.isAccountsUpdateSuccess = false;
            }
            else
            {
                bool isBalanceEnough = await _accountRepository
                                .CheckBalance(message.FromAccountId, message.Amount);

                if (isBalanceEnough == false)
                {
                    accountsUpdated.FailureReason = $"Account Id: {message.FromAccountId} no have enough balance";
                    accountsUpdated.isAccountsUpdateSuccess = false;
                }
                else
                {
                    await _accountRepository.UpdateAccounts(message);
                    accountsUpdated.isAccountsUpdateSuccess = true;
                }
            }
            //AccountsUpdated accountsUpdated = await CreateTransaction(message);
            await context.Publish(accountsUpdated)
                .ConfigureAwait(false);
        }

        //private async Task<AccountsUpdated> CreateTransaction(UpdateAccounts transaction)
        //{
        //    AccountsUpdated transactionCorrectness = await AccountIdsCorrectness(transaction.FromAccountId, transaction.ToAccountId);
        //    if (transactionCorrectness.isAccountsUpdateSuccess == true)
        //    {
        //        transactionCorrectness = await HasBalance(transaction.FromAccountId, transaction.Amount);
        //        if (transactionCorrectness.isAccountsUpdateSuccess == true)
        //        {
        //            await DoTransaction(transaction);
        //        }
        //    }
        //    return new AccountsUpdated()
        //    {
        //        TransactionId = transaction.TransactionId,
        //        isAccountsUpdateSuccess = transactionCorrectness.isAccountsUpdateSuccess,
        //        FailureReason = transactionCorrectness.FailureReason
        //    };
        //}
        //private async Task<AccountsUpdated> AccountIdsCorrectness(Guid fromAccountId, Guid toAccountId)
        //{
        //    AccountsUpdated transactionCorrectness = new AccountsUpdated();
        //    if (await _accountRepository.CheckAccountCorrectness(fromAccountId) == false)
        //    {
        //        transactionCorrectness.isAccountsUpdateSuccess = false;
        //        transactionCorrectness.FailureReason = "The fromAccountId doesn't exist";
        //    }
        //    else if (await _accountRepository.CheckAccountCorrectness(toAccountId) == false)
        //    {
        //        transactionCorrectness.isAccountsUpdateSuccess = false;
        //        transactionCorrectness.FailureReason = "The toAccountId doesn't exist";
        //    }
        //    return transactionCorrectness;
        //}

        //private async Task<AccountsUpdated> HasBalance(Guid fromAccountId, int amount)
        //{
        //    AccountsUpdated transactionCorrectness = new AccountsUpdated();
        //    bool isBbalanceEnough = await _accountRepository.CheckBalance(fromAccountId, amount);
        //    if (isBbalanceEnough == false)
        //    {
        //        transactionCorrectness.isAccountsUpdateSuccess = false;
        //        transactionCorrectness.FailureReason = $"There is not enough money in the account. accountId: {fromAccountId}";
        //    }
        //    return transactionCorrectness;
        //}

        //private Task DoTransaction(UpdateAccounts transaction)
        //{
        //    _accountRepository.UpdateAccounts(transaction);
        //    return Task.CompletedTask;
        //}
    }
}

