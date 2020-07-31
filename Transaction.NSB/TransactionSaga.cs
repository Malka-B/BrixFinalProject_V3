using Messages.Commands;
using Messages.Events;
using NServiceBus;
using System.Threading.Tasks;

namespace Transaction.NSB
{
    public class TransactionSaga : Saga<TransactionData>,
        IAmStartedByMessages<StartTransaction>,
        IHandleMessages<AccountsUpdated>
    {
        public async Task Handle(StartTransaction message, IMessageHandlerContext context)
        {
            UpdateAccounts updateAccounts = new UpdateAccounts()
            {
                Amount = message.Amount,
                FromAccountId = message.FromAccountId,
                ToAccountId = message.ToAccountId,
                TransactionId = message.TransactionId,
                Date = message.Date
            };
            await context.Send(updateAccounts)
                .ConfigureAwait(false);
        }

        public async Task Handle(AccountsUpdated message, IMessageHandlerContext context)
        {
            UpdateTransactionStatus updateTransactionStatus = new UpdateTransactionStatus()
            {
                IsTransactionSucceeded = message.isAccountsUpdateSuccess,
                TransactionId = message.TransactionId,
                FailureReason = message.FailureReason
            };
            await context.SendLocal(updateTransactionStatus)
                .ConfigureAwait(false);
            MarkAsComplete();
        }


        protected override void ConfigureHowToFindSaga(SagaPropertyMapper<TransactionData> mapper)
        {
            mapper.ConfigureMapping<StartTransaction>(message => message.TransactionId)
                .ToSaga(sagaData => sagaData.TransactionId);
            mapper.ConfigureMapping<AccountsUpdated>(message => message.TransactionId)
               .ToSaga(sagaData => sagaData.TransactionId);
        }
    }
}
