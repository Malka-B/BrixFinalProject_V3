using Account.Share.Interfaces;
using Messages.Commands;
using Messages.Events;
using NServiceBus;
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
            bool areAccountsExist = await _accountRepository
                .CheckAccountsCorrectness(message.FromAccountId, message.ToAccountId);
            if (areAccountsExist == false)
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
            await context.Publish(accountsUpdated)
                .ConfigureAwait(false);
        }
    }
}


