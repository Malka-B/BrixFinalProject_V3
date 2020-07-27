using NServiceBus;
using System;
using System.Threading.Tasks;
using Messages.Commands;
using Messages.Events;

namespace Transaction.NSB
{
    public class TransactionSaga : Saga<TransactionData>,
        IAmStartedByMessages<StartTransaction>,
        IHandleMessages<AccountsUpdated>//,
       // IHandleMessages<TransactionUpdated>
    {
        //define routing at prigram
        public async Task Handle(StartTransaction message, IMessageHandlerContext context)
        {
            UpdateAccounts updateAccounts = new UpdateAccounts()
            {
                Amount = message.Amount,
                FromAccountId = message.FromAccountId,
                ToAccountId = message.ToAccountId,
                TransactionId = message.TransactionId
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

        //public Task Handle(TransactionUpdated message, IMessageHandlerContext context)
        //{
        //    Func1();
        //    //MarkAsComplete();
        //}
        //private void Func1()
        //{
        //    MarkAsComplete();
        //}


        protected override void ConfigureHowToFindSaga(SagaPropertyMapper<TransactionData> mapper)
        {
            mapper.ConfigureMapping<StartTransaction>(message => message.TransactionId)
                .ToSaga(sagaData => sagaData.TransactionId);
            mapper.ConfigureMapping<AccountsUpdated>(message => message.TransactionId)
               .ToSaga(sagaData => sagaData.TransactionId);
            //mapper.ConfigureMapping<TransactionUpdated>(message => message.TransactionId)
            //  .ToSaga(sagaData => sagaData.TransactionId);
        }
    }
}
