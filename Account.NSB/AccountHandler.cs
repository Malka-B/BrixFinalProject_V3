using Account.Share.Interfaces;
using Account.Share.Models;
using AutoMapper;
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
        private readonly IMapper _mapper;

        public AccountHendler(IAccountRepository accountRepository, IMapper mapper)
        {
            _accountRepository = accountRepository;
            _mapper = mapper;
        }

        public async Task Handle(UpdateAccounts message, IMessageHandlerContext context)
        {
            AccountsUpdated accountsUpdated = new AccountsUpdated
            {
                TransactionId = message.TransactionId
            };

            TransactionDetailsForHistory transactionDetails = _mapper.Map<TransactionDetailsForHistory>(message);
             
            AccountsUpdated transactionCorrectness = await AccountIdsCorrectness(message.FromAccountId, message.ToAccountId);
            if (transactionCorrectness.isAccountsUpdateSuccess == true)
            {
                transactionCorrectness = await CheckBalance(message.FromAccountId, message.Amount);
                if (transactionCorrectness.isAccountsUpdateSuccess == true)
                {
                    AccountsBalance accountsBalance = await DoTransaction(message);
                    transactionDetails.BalanceOfFromAccount = accountsBalance.BalanceOfFromAccount;
                    transactionDetails.BalanceOfToAccount = accountsBalance.BalanceOfToAccount;
                }
            }
            if (transactionCorrectness.isAccountsUpdateSuccess)
            {               
                UpdateSucceededTransaction transactionSucceeded = _mapper.Map<UpdateSucceededTransaction>(message);
                transactionSucceeded.BalanceOfToAccount = transactionDetails.BalanceOfToAccount;
                transactionSucceeded.BalanceOfFromAccount = transactionDetails.BalanceOfFromAccount;
                await context.SendLocal(transactionSucceeded);
            }
            else
            {                
                UpdateFailedTransaction transactionFailed = _mapper.Map<UpdateFailedTransaction>(message);
                transactionFailed.FailureReason = transactionCorrectness.FailureReason;
                await context.SendLocal(transactionFailed);
            }
            accountsUpdated.isAccountsUpdateSuccess = transactionCorrectness.isAccountsUpdateSuccess;
            accountsUpdated.FailureReason = transactionCorrectness.FailureReason;
            await context.Publish(accountsUpdated)
                .ConfigureAwait(false);
        }
                
        private async Task<AccountsUpdated> AccountIdsCorrectness(Guid fromAccountId, Guid toAccountId)
        {
            AccountsUpdated transactionCorrectness = new AccountsUpdated(true);
            if (await _accountRepository.CheckAccountCorrectness(fromAccountId) == false)
            {
                transactionCorrectness.isAccountsUpdateSuccess = false;
                transactionCorrectness.FailureReason = $"The FromAccountId: {fromAccountId} doesn't exist";
            }
            else if (await _accountRepository.CheckAccountCorrectness(toAccountId) == false)
            {
                transactionCorrectness.isAccountsUpdateSuccess = false;
                transactionCorrectness.FailureReason = $"The ToAccountId: {toAccountId} doesn't exist";
            }
            return transactionCorrectness;
        }

        private async Task<AccountsUpdated> CheckBalance(Guid fromAccountId, int amount)
        {
            AccountsUpdated transactionCorrectness = new AccountsUpdated(true);
            int balance = await _accountRepository.CheckBalance(fromAccountId, amount);
            if(balance < amount)
            {
                transactionCorrectness.isAccountsUpdateSuccess = false;
                transactionCorrectness.FailureReason = $"There is not enough money in the account. accountId: {fromAccountId}, CurrentBalance: {balance}";
            }
            return transactionCorrectness;
        }

        private async Task<AccountsBalance> DoTransaction(UpdateAccounts transaction)
        {           
            return await _accountRepository.UpdateAccounts(transaction);
        }
    }
}

